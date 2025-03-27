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

    async void Awake()
	{
		try
		{
            var options = new InitializationOptions();
            options.SetProfile("Player" + UnityEngine.Random.Range(0, 100));
            await UnityServices.InitializeAsync(options);
		}
		catch (Exception e)
		{
			Debug.LogException(e);
		}
		SetupEvents();
		allDecks = JsonConvert.DeserializeObject<Dictionary<string, Deck>>(PlayerPrefs.GetString("Decks"));
		DontDestroyOnLoad(gameObject);
	}

	// Setup authentication event handlers if desired
	void SetupEvents()
	{
		AuthenticationService.Instance.SignedIn += () => {
			// Shows how to get a playerID
			Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

			// Shows how to get an access token
			Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");

		};

		AuthenticationService.Instance.SignInFailed += (err) => {
			Debug.LogError(err);
		};

		AuthenticationService.Instance.SignedOut += () => {
			Debug.Log("Player signed out.");
		};

		AuthenticationService.Instance.Expired += () =>
		{
			Debug.Log("Player session could not be refreshed and expired.");
		};
	}

	public async Task SignUpWithUsernamePasswordAsync(string username, string password)
	{
		try
		{
			await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
			Debug.Log("SignUp is successful.");
		}
		catch (AuthenticationException ex)
		{
			// Compare error code to AuthenticationErrorCodes
			// Notify the player with the proper error message
			Debug.LogException(ex);
		}
		catch (RequestFailedException ex)
		{
			// Compare error code to CommonErrorCodes
			// Notify the player with the proper error message
			Debug.LogException(ex);
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
			// Compare error code to AuthenticationErrorCodes
			// Notify the player with the proper error message
			Debug.LogException(ex);
		}
		catch (RequestFailedException ex)
		{
			// Compare error code to CommonErrorCodes
			// Notify the player with the proper error message
			Debug.LogException(ex);
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
			// Compare error code to AuthenticationErrorCodes
			// Notify the player with the proper error message
			Debug.LogException(ex);
		}
		catch (RequestFailedException ex)
		{
			// Compare error code to CommonErrorCodes
			// Notify the player with the proper error message
			Debug.LogException(ex);
		}
	}

	public async void SaveData()
	{
		var playerData = new Dictionary<string, object>{
		  {"firstKeyName", "a text value"},
		  {"secondKeyName", 123}
		};
		await CloudSaveService.Instance.Data.Player.SaveAsync(playerData);
		Debug.Log($"Saved data {string.Join(',', playerData)}");
	}

	public async void LoadData()
	{
		var playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> {
		  "firstKeyName", "secondKeyName"
		});

		if (playerData.TryGetValue("firstKeyName", out var firstKey))
		{
			Debug.Log($"firstKeyName value: {firstKey.Value.GetAs<string>()}");
		}

		if (playerData.TryGetValue("secondKeyName", out var secondKey))
		{
			Debug.Log($"secondKey value: {secondKey.Value.GetAs<int>()}");
		}
	}

}
