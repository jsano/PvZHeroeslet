using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using System;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using Unity.Services.CloudSave;
using static DeckBuilder;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.Services.Leaderboards;
using Unity.Services.CloudSave.Models.Data.Player;

public class UserAccounts : MonoBehaviour
{

    public static class GameStats
    {
		public static string DeckName;
        public static int PlantHero { get; set; }
        public static int[] Superpowers { get; set; }

		public static int ZombieHero { get; set; }
    }

	public static Dictionary<string, Deck> allDecks = new();
	public static int profilePicture;

    public static UserAccounts Instance;

    public GameObject Error;
	private GameObject errorInstance;
	private Coroutine c;

	/// <summary>
	/// Local cache of [player score, player tier] so that it can be instantly loaded without calling Leaderboards API.
	/// Must be updated properly using <c>UpdateCachedScore</c>
	/// </summary>
	public string[] CachedScore { get; private set; }

    async void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
			return;
		}
		else Instance = this;

		try
		{
            await UnityServices.InitializeAsync();
		}
		catch (Exception e)
		{
			Debug.LogException(e);
		}
		
		//AuthenticationService.Instance.ClearSessionToken();
        
		errorInstance = Instantiate(Error, transform);

        SetupEvents();
		await SignInCachedUserAsync();
		DontDestroyOnLoad(gameObject);
	}

	// Setup authentication event handlers if desired
	void SetupEvents()
	{
		AuthenticationService.Instance.SignedIn += () => {
			// Shows how to get a playerID
			Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

            // Shows how to get a player name
            Debug.Log($"PlayerName: {AuthenticationService.Instance.PlayerName}");

            // Shows how to get an access token
            Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");
		};

		AuthenticationService.Instance.SignInFailed += (err) => {
			
		};

		AuthenticationService.Instance.SignedOut += () => {
			Debug.Log("Player signed out.");
            CachedScore = null;
            SceneManager.LoadScene("Login");
        };

		AuthenticationService.Instance.Expired += async () =>
		{
			Debug.Log("Player session could not be refreshed and expired.");
            await SignInCachedUserAsync();
        };
	}

	public async Task SignUpWithUsernamePasswordAsync(string username, string password, string displayName)
	{
		try
		{
			await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
			await AuthenticationService.Instance.UpdatePlayerNameAsync(displayName);
			Debug.Log("SignUp is successful. Display name: " + AuthenticationService.Instance.PlayerName);
            CachedScore = new string[] { "0", "WOOD"};

			StrategyDecks a = new();
            await LeaderboardsService.Instance.AddPlayerScoreAsync("devplayers", 0);
        }
		catch (AuthenticationException ex)
		{
            ShowError(ex.Message);
        }
		catch (RequestFailedException ex)
		{
            ShowError(ex.Message);
        }
	}

	public async Task SignInWithUsernamePasswordAsync(string username, string password)
	{
		try
		{
			await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);
			Debug.Log("SignIn is successful.");
            await LoadData();
            UpdateCachedScore();
        }
		catch (AuthenticationException ex)
		{
			ShowError(ex.Message);
		}
		catch (RequestFailedException ex)
		{
			ShowError(ex.Message);
		}
	}

    async Task SignInCachedUserAsync()
    {
        // Check if a cached player already exists by checking if the session token exists
        if (!AuthenticationService.Instance.SessionTokenExists)
        {
            // if not, then do nothing
            return;
        }

        // Sign in Anonymously
        // This call will sign in the cached player.
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Cached SignIn is successful");
            await LoadData();
            UpdateCachedScore();
        }
        catch (AuthenticationException ex)
        {
            ShowError(ex.Message);
        }
        catch (RequestFailedException ex)
        {
            ShowError(ex.Message);
        }
    }

    public async Task UpdatePasswordAsync(string currentPassword, string newPassword)
	{
		try
		{
			await AuthenticationService.Instance.UpdatePasswordAsync(currentPassword, newPassword);
			Debug.Log("Password updated.");
		}
		catch (AuthenticationException ex)
		{
			ShowError(ex.Message);
		}
		catch (RequestFailedException ex)
		{
			ShowError(ex.Message);
		}
	}

	public async Task SaveData()
	{
		await CloudSaveService.Instance.Data.Player.SaveAsync(new Dictionary<string, object> { { "Decks", JsonConvert.SerializeObject(allDecks) } });
        await CloudSaveService.Instance.Data.Player.SaveAsync(new Dictionary<string, object> { { "Profile", profilePicture } }, new Unity.Services.CloudSave.Models.Data.Player.SaveOptions(new PublicWriteAccessClassOptions()));
		Debug.Log("Saved user data");
	}

	public async Task LoadData()
	{
		var playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> {"Decks"});
		if (playerData.TryGetValue("Decks", out var firstKey))
		{
			allDecks = JsonConvert.DeserializeObject<Dictionary<string, Deck>>(firstKey.Value.GetAs<string>()); 
			Debug.Log("Loaded user decks");
		}
        playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { "Profile" }, new LoadOptions(new PublicReadAccessClassOptions()));
        if (playerData.TryGetValue("Profile", out var secondKey))
        {
			profilePicture = secondKey.Value.GetAs<int>();
            Debug.Log(profilePicture);
        }
    }

	public void ShowError(string message)
	{
		errorInstance.SetActive(true);
		errorInstance.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = message;
		if (c != null) StopCoroutine(c);
		c = StartCoroutine(RemoveError());
	}

	private IEnumerator RemoveError()
	{
		yield return new WaitForSeconds(3);
		errorInstance.SetActive(false);
	}

	public async void UpdateCachedScore()
	{
		try
		{
			var response = await LeaderboardsService.Instance.GetPlayerScoreAsync("devplayers");
			CachedScore = new string[] { response.Score + "", response.Tier };
		}
		catch (Exception)
		{
            CachedScore = new string[] { "0", "WOOD" };
        }
    }

}
