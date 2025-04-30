using System;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.CloudSave.Models.Data.Player;
using Unity.Services.CloudSave;
using UnityEngine;
using UnityEngine.UI;

public class Profile : MonoBehaviour
{

    public Image pfp;
    public TextMeshProUGUI usernameText;
    public TextMeshProUGUI tier;
    public TextMeshProUGUI trophies;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (GetComponent<Button>().interactable) LoadProfileThumbnail(AuthenticationService.Instance.PlayerId, AuthenticationService.Instance.PlayerName, UserAccounts.Instance.CachedScore[0], UserAccounts.Instance.CachedScore[1]);
    }

    public async void LoadProfileThumbnail(string playerID, string username, string score, string rank)
    {
        usernameText.text = username.Substring(0, username.IndexOf("#"));
        trophies.text = score;
        tier.text = rank;
        if (playerID == AuthenticationService.Instance.PlayerId) pfp.sprite = ProfilePictureIDToSprite(UserAccounts.profilePicture);
        else
        {
            var playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { "Profile" }, new LoadOptions(new PublicReadAccessClassOptions(playerID)));
            if (playerData.TryGetValue("Profile", out var keyName)) pfp.sprite = ProfilePictureIDToSprite(keyName.Value.GetAs<int>());
        }
    }

    public void LoadProfile()
    {
        ProfileInfo profileInfo = FindAnyObjectByType<ProfileInfo>(FindObjectsInactive.Include).GetComponent<ProfileInfo>();
        profileInfo.Show(AuthenticationService.Instance.PlayerId, AuthenticationService.Instance.PlayerName, UserAccounts.Instance.CachedScore[0], UserAccounts.Instance.CachedScore[1]);
    }

    public static Sprite ProfilePictureIDToSprite(int ID)
    {
        if (ID > AllCards.Instance.cards.Length)
        {
            return AllCards.Instance.heroes[ID - AllCards.Instance.cards.Length].GetComponent<SpriteRenderer>().sprite;
        }
        else return AllCards.Instance.cards[ID].GetComponent<SpriteRenderer>().sprite;
    }

}

/*
[CloudCodeFunction("GetName")]
    public async Task<string> GetName(IExecutionContext context, IGameApiClient gameApiClient, string playerId)
    {
        var response = await gameApiClient.PlayerNamesApi.GetNameAsync(context , context.ServiceToken, playerId);
        return response.Data.Name;
    }
*/