using System;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.CloudSave.Models.Data.Player;
using Unity.Services.CloudSave;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.CloudCode;
using Unity.Services.Leaderboards.Exceptions;
using Unity.Services.Leaderboards;
using System.Threading.Tasks;

public class ProfileThumbnail : MonoBehaviour
{

    public Image pfp;
    public TextMeshProUGUI usernameText;
    public TextMeshProUGUI tier;
    public TextMeshProUGUI trophies;
    private string ID = null;
    private string username;
    private string score;
    private string rank;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (ID == null) LoadProfileThumbnail(AuthenticationService.Instance.PlayerId, AuthenticationService.Instance.PlayerName, UserAccounts.Instance.CachedScore[0], UserAccounts.Instance.CachedScore[1]);
    }

    public async Task LoadProfileThumbnail(string playerID, string username, string score, string rank)
    {
        ID = playerID;
        this.username = username;
        this.score = score;
        this.rank = rank;
        usernameText.text = username.Substring(0, username.IndexOf("#"));
        trophies.text = score;
        tier.text = rank;
        if (playerID == AuthenticationService.Instance.PlayerId) pfp.sprite = ProfilePictureIDToSprite(UserAccounts.profilePicture);
        else
        {
            var playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { "Profile" }, new LoadOptions(new PublicReadAccessClassOptions(playerID)));
            if (playerData.TryGetValue("Profile", out var keyName)) pfp.sprite = ProfilePictureIDToSprite(keyName.Value.GetAs<int>());
            else pfp.sprite = ProfilePictureIDToSprite(0);
        }
    }

    public async void LoadProfileThumbnail(string playerID)
    {
        ID = playerID;
        string name = "Player";
        string score = "0";
        string tier = "WOOD";
        try
        {
            var entry = await LeaderboardsService.Instance.GetScoresByPlayerIdsAsync("devplayers", new() { playerID });
            name = entry.Results[0].PlayerName;
            score = entry.Results[0].Score + "";
            tier = entry.Results[0].Tier;
        }
        catch (LeaderboardsException)
        {
            try
            {
                var response = await CloudCodeService.Instance.CallEndpointAsync<string>("GetNameByPlayerID", new Dictionary<string, object>() { { "ID", playerID } });
                name = response;
            }
            catch (CloudCodeException e)
            {
                Debug.Log(e.Reason);
            }
        }
        await LoadProfileThumbnail(playerID, name, score, tier);
    }

    public async void Refresh()
    {
        await LoadProfileThumbnail(ID, username, score, rank);
    }

    public void LoadProfile()
    {
        ProfileInfo profileInfo = FindAnyObjectByType<ProfileInfo>(FindObjectsInactive.Include).GetComponent<ProfileInfo>();
        profileInfo.Show(ID, username, score, rank);
    }

    public static Sprite ProfilePictureIDToSprite(int ID)
    {
        if (ID >= AllCards.Instance.cards.Length)
        {
            return AllCards.Instance.heroes[ID - AllCards.Instance.cards.Length].GetComponent<SpriteRenderer>().sprite;
        }
        else return AllCards.Instance.cards[ID].GetComponent<SpriteRenderer>().sprite;
    }

}
