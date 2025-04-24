using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using System;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models.Data.Player;
using static DeckBuilder;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using TMPro;

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

    public static UserAccounts Instance;

    public GameObject Error;
	private GameObject errorInstance;
	private Coroutine c;

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
		AuthenticationService.Instance.SignedIn += async () => {
			// Shows how to get a playerID
			Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

            // Shows how to get a player name
            Debug.Log($"PlayerName: {AuthenticationService.Instance.PlayerName}");

            // Shows how to get an access token
            Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");

			await LoadData();
			if (SceneManager.GetActiveScene().name != "Testing") SceneManager.LoadScene("Start");
		};

		AuthenticationService.Instance.SignInFailed += (err) => {
			
		};

		AuthenticationService.Instance.SignedOut += () => {
			Debug.Log("Player signed out.");
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
		Debug.Log("Saved user decks");
	}

	public async Task LoadData()
	{
		var playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> {"Decks"});
		if (playerData.TryGetValue("Decks", out var firstKey))
		{
			allDecks = JsonConvert.DeserializeObject<Dictionary<string, Deck>>(firstKey.Value.GetAs<string>()); 
			Debug.Log("Loaded user decks");
			Debug.Log(allDecks);
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

}
