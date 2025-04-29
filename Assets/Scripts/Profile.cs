using System;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Leaderboards;
using UnityEngine;
using UnityEngine.UI;

public class Profile : MonoBehaviour
{

    public Image pfp;
    public TextMeshProUGUI username;
    public TextMeshProUGUI tier;
    public TextMeshProUGUI trophies;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //pfp.sprite = ;
        string temp = AuthenticationService.Instance.PlayerName;
        username.text = temp.Substring(0, temp.IndexOf("#"));
        trophies.text = UserAccounts.Instance.CachedScore[0];
        tier.text = UserAccounts.Instance.CachedScore[1];
    }

    public void LoadProfile()
    {

    }

}
