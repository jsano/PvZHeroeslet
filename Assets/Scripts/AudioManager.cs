using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 1f;

    [Range(0.1f, 3f)]
    public float pitch = 1f;

    [Range(0f, 1f)]
    public float spatialBlend = 0f;

    public bool loop = false;

    [HideInInspector]
    public AudioSource source;

    [HideInInspector]
    public AudioMixerGroup outputAudioMixerGroup;

    public string partnerName;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Sound Collections")]
    [SerializeField] private Sound[] music;
    [SerializeField] private Sound[] sfx;

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixerGroup musicMixerGroup;
    [SerializeField] private AudioMixerGroup sfxMixerGroup;

    [Header("Settings")]
    [SerializeField] private float musicFadeDuration = 1.0f;

    private Sound currentMusic;
    private Dictionary<string, Sound> musicDict = new Dictionary<string, Sound>();
    private Dictionary<string, Sound> sfxDict = new Dictionary<string, Sound>();

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            InitializeAudioSources();
            DontDestroyOnLoad(gameObject);

            SetMusicVolume(PlayerPrefs.GetFloat("music", 1));
            SetSFXVolume(PlayerPrefs.GetFloat("sfx", 1));
        }
        else Destroy(gameObject);
    }

    private void InitializeAudioSources()
    {
        // Initialize music dictionary and audio sources
        foreach (Sound s in music)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.spatialBlend = s.spatialBlend;

            // Assign mixer group
            s.source.outputAudioMixerGroup = s.outputAudioMixerGroup != null ? s.outputAudioMixerGroup : musicMixerGroup;

            // Add to dictionary for quick access
            if (!musicDict.ContainsKey(s.name)) musicDict.Add(s.name, s);
            else Debug.LogWarning($"Duplicate music name found: {s.name}. Only the first one will be accessible.");
        }

        // Initialize SFX dictionary and audio sources
        foreach (Sound s in sfx)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.spatialBlend = s.spatialBlend;

            // Assign mixer group
            s.source.outputAudioMixerGroup = s.outputAudioMixerGroup != null ? s.outputAudioMixerGroup : sfxMixerGroup;

            // Add to dictionary for quick access
            if (!sfxDict.ContainsKey(s.name)) sfxDict.Add(s.name, s);
            else Debug.LogWarning($"Duplicate SFX name found: {s.name}. Only the first one will be accessible.");
        }
    }

    /// <summary>
    /// Play a music track by name. Stops any currently playing music unless it's the same one.
    /// </summary>
    /// <param name="name">Name of the music track to play</param>
    /// <param name="fadeDuration">Optional fade duration (overrides default)</param>
    public void PlayMusic(string name, float? fadeDuration = null)
    {
        if (!musicDict.TryGetValue(name, out Sound s))
        {
            Debug.LogWarning($"Music {name} not found!");
            return;
        }
        
        if (currentMusic != null && currentMusic.name == name) return;
        
        float actualFadeDuration = fadeDuration ?? musicFadeDuration;

        // If there's music already playing, fade it out and then fade in the new one
        if (currentMusic != null && currentMusic.source.isPlaying)
        {
            StartCoroutine(FadeMusicTransition(currentMusic, s, actualFadeDuration));
            if (currentMusic.partnerName != "") StartCoroutine(FadeMusicOut(musicDict[currentMusic.partnerName], actualFadeDuration));
        }
        else
        {
            // No music playing, just fade in the new one
            StartCoroutine(FadeMusicIn(s, actualFadeDuration));
        }

        currentMusic = s;

        if (currentMusic.partnerName != "")
        {
            musicDict[currentMusic.partnerName].source.volume = 0;
            musicDict[currentMusic.partnerName].source.Play();
        }
    }

    /// <summary>
    /// Stop the currently playing music with an optional fade out.
    /// </summary>
    /// <param name="fadeDuration">Optional fade duration (overrides default)</param>
    public void StopMusic(float? fadeDuration = null)
    {
        if (currentMusic != null && currentMusic.source.isPlaying)
        {
            float actualFadeDuration = fadeDuration ?? musicFadeDuration;
            StartCoroutine(FadeMusicOut(currentMusic, actualFadeDuration));
            if (currentMusic.partnerName != "") StartCoroutine(FadeMusicOut(musicDict[currentMusic.partnerName], actualFadeDuration));
            currentMusic = null;
        }
    }

    /// <summary>
    /// Set the volume of all music.
    /// </summary>
    /// <param name="volume">Volume level (0-1)</param>
    public void SetMusicVolume(float volume)
    {
        if (musicMixerGroup != null)
        {
            // Convert to mixer dB scale (-80dB to 0dB)
            float dBValue = volume > 0 ? Mathf.Log10(volume) * 20 : -80f;
            musicMixerGroup.audioMixer.SetFloat("MusicVolume", dBValue);
        }
        else
        {
            // Directly adjust each source if no mixer
            foreach (var music in musicDict.Values)
            {
                music.source.volume = volume * music.volume;
            }
        }
        PlayerPrefs.SetFloat("music", volume);
    }

    public IEnumerator ToggleBattleMusic(bool battleMusic)
    {
        Sound toFadeOut = battleMusic ? currentMusic : musicDict[currentMusic.partnerName];
        Sound toFadeIn = battleMusic ? musicDict[currentMusic.partnerName] : currentMusic;
        
        float startVolume = toFadeOut.source.volume;
        float targetVolume = toFadeIn.volume;
        float timer = 0;
        while (timer < musicFadeDuration)
        {
            timer += Time.deltaTime;
            toFadeOut.source.volume = Mathf.Lerp(startVolume, 0, timer / musicFadeDuration);
            toFadeIn.source.volume = Mathf.Lerp(0, targetVolume, timer / musicFadeDuration);
            yield return null;
        }
        toFadeOut.source.volume = 0;
        toFadeIn.source.volume = targetVolume;
    }

    private IEnumerator FadeMusicTransition(Sound oldMusic, Sound newMusic, float duration)
    {
        // Start fading out the old music
        yield return FadeMusicOut(oldMusic, duration);

        // Start the new music
        yield return StartCoroutine(FadeMusicIn(newMusic, duration));
    }

    private IEnumerator FadeMusicIn(Sound music, float duration)
    {
        music.source.volume = 0;
        music.source.Play();

        float targetVolume = music.volume;
        float timer = 0;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            music.source.volume = Mathf.Lerp(0, targetVolume, timer / duration);
            yield return null;
        }

        music.source.volume = targetVolume; // Ensure we end at the exact target
    }

    private IEnumerator FadeMusicOut(Sound music, float duration)
    {
        float startVolume = music.source.volume;
        float timer = 0;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            music.source.volume = Mathf.Lerp(startVolume, 0, timer / duration);
            yield return null;
        }

        music.source.Stop();
        music.source.volume = music.volume; // Reset for next play
    }

    /// <summary>
    /// Play a sound effect by name.
    /// </summary>
    /// <param name="name">Name of the sound effect</param>
    /// <param name="volumeScale">Optional volume scaling factor</param>
    /// <param name="pitchVariation">Optional random pitch variation (-pitchVariation to +pitchVariation)</param>
    /// <returns>The AudioSource that's playing the sound (for further control)</returns>
    public AudioSource PlaySFX(string name, float volumeScale = 1f, float pitchVariation = 0f)
    {
        if (!sfxDict.TryGetValue(name, out Sound s))
        {
            Debug.LogWarning($"SFX {name} not found!");
            return null;
        }

        // Adjust pitch if variation is requested
        if (pitchVariation > 0)
        {
            float randomPitch = UnityEngine.Random.Range(-pitchVariation, pitchVariation);
            s.source.pitch = s.pitch * (1 + randomPitch);
        }
        else
        {
            s.source.pitch = s.pitch;
        }

        // Adjust volume if scaling is requested
        s.source.volume = s.volume * volumeScale;

        s.source.Play();
        return s.source;
    }

    /// <summary>
    /// Play a sound effect at a specific position in 3D space.
    /// </summary>
    /// <param name="name">Name of the sound effect</param>
    /// <param name="position">Position in world space</param>
    /// <param name="volumeScale">Optional volume scaling factor</param>
    /// <param name="pitchVariation">Optional random pitch variation</param>
    public void PlaySFXAtPosition(string name, Vector3 position, float volumeScale = 1f, float pitchVariation = 0f)
    {
        if (!sfxDict.TryGetValue(name, out Sound s))
        {
            Debug.LogWarning($"SFX {name} not found!");
            return;
        }

        // Create a temporary game object at the position
        GameObject tempGO = new GameObject("TempAudio");
        tempGO.transform.position = position;

        // Add audio source and set its properties
        AudioSource tempSource = tempGO.AddComponent<AudioSource>();
        tempSource.clip = s.clip;
        tempSource.outputAudioMixerGroup = s.source.outputAudioMixerGroup;

        // Set to full 3D
        tempSource.spatialBlend = 1f;

        // Apply volume and pitch
        if (pitchVariation > 0)
        {
            float randomPitch = UnityEngine.Random.Range(-pitchVariation, pitchVariation);
            tempSource.pitch = s.pitch * (1 + randomPitch);
        }
        else
        {
            tempSource.pitch = s.pitch;
        }

        tempSource.volume = s.volume * volumeScale;

        // Play and destroy when finished
        tempSource.Play();
        Destroy(tempGO, s.clip.length / tempSource.pitch + 0.1f);
    }

    /// <summary>
    /// Stop a specific SFX from playing.
    /// </summary>
    /// <param name="name">Name of the sound effect</param>
    public void StopSFX(string name)
    {
        if (sfxDict.TryGetValue(name, out Sound s))
        {
            s.source.Stop();
        }
    }

    /// <summary>
    /// Set the volume of all sound effects.
    /// </summary>
    /// <param name="volume">Volume level (0-1)</param>
    public void SetSFXVolume(float volume)
    {
        if (sfxMixerGroup != null)
        {
            // Convert to mixer dB scale (-80dB to 0dB)
            float dBValue = volume > 0 ? Mathf.Log10(volume) * 20 : -80f;
            sfxMixerGroup.audioMixer.SetFloat("SFXVolume", dBValue);
        }
        else
        {
            // Directly adjust each source if no mixer
            foreach (var sfx in sfxDict.Values)
            {
                sfx.source.volume = volume * sfx.volume;
            }
        }
        PlayerPrefs.SetFloat("sfx", volume);
    }

    /// <summary>
    /// Add a new music track at runtime (e.g., from downloaded content)
    /// </summary>
    /// <param name="name">Unique identifier for the music</param>
    /// <param name="clip">AudioClip to play</param>
    /// <param name="volume">Volume level (0-1)</param>
    /// <param name="loop">Whether the music should loop</param>
    /// <returns>True if added successfully</returns>
    public bool AddMusic(string name, AudioClip clip, float volume = 1f, bool loop = true)
    {
        if (musicDict.ContainsKey(name))
        {
            Debug.LogWarning($"Music {name} already exists!");
            return false;
        }

        Sound newSound = new Sound
        {
            name = name,
            clip = clip,
            volume = volume,
            pitch = 1f,
            loop = loop,
            outputAudioMixerGroup = musicMixerGroup
        };

        // Create audio source component
        newSound.source = gameObject.AddComponent<AudioSource>();
        newSound.source.clip = clip;
        newSound.source.volume = volume;
        newSound.source.pitch = 1f;
        newSound.source.loop = loop;
        newSound.source.outputAudioMixerGroup = musicMixerGroup;

        // Add to dictionary
        musicDict.Add(name, newSound);

        return true;
    }

    /// <summary>
    /// Add a new sound effect at runtime
    /// </summary>
    /// <param name="name">Unique identifier for the SFX</param>
    /// <param name="clip">AudioClip to play</param>
    /// <param name="volume">Volume level (0-1)</param>
    /// <returns>True if added successfully</returns>
    public bool AddSFX(string name, AudioClip clip, float volume = 1f)
    {
        if (sfxDict.ContainsKey(name))
        {
            Debug.LogWarning($"SFX {name} already exists!");
            return false;
        }

        Sound newSound = new Sound
        {
            name = name,
            clip = clip,
            volume = volume,
            pitch = 1f,
            loop = false,
            outputAudioMixerGroup = sfxMixerGroup
        };

        // Create audio source component
        newSound.source = gameObject.AddComponent<AudioSource>();
        newSound.source.clip = clip;
        newSound.source.volume = volume;
        newSound.source.pitch = 1f;
        newSound.source.loop = false;
        newSound.source.outputAudioMixerGroup = sfxMixerGroup;

        // Add to dictionary
        sfxDict.Add(name, newSound);

        return true;
    }

    /// <summary>
    /// Load and add audio from a file or resource
    /// </summary>
    public IEnumerator LoadAudioClip(string path, string name, bool isMusic, Action<bool> callback = null)
    {
        using (UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequestMultimedia.GetAudioClip(path, AudioType.UNKNOWN))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
            {
                AudioClip clip = UnityEngine.Networking.DownloadHandlerAudioClip.GetContent(www);

                bool success;
                if (isMusic)
                {
                    success = AddMusic(name, clip);
                }
                else
                {
                    success = AddSFX(name, clip);
                }

                callback?.Invoke(success);
            }
            else
            {
                Debug.LogError($"Error loading audio clip: {www.error}");
                callback?.Invoke(false);
            }
        }
    }

}
