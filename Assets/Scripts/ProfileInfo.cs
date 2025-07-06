using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.UI;

public class ProfileInfo : MonoBehaviour
{
	public ProfileThumbnail p;
    public Button exit;
	public GameObject edit;
	public GameObject changePFP;
	public Transform content;
	public static int chosenPFP;
	public GameObject PFPButton;

    public async void Show(string playerID, string username, string score, string rank)
	{
        if (isActiveAndEnabled) return;
		await p.LoadProfileThumbnail(playerID, username, score, rank);
		transform.parent.gameObject.SetActive(true);
		if (playerID != AuthenticationService.Instance.PlayerId) edit.SetActive(false);
		else edit.SetActive(true);
    }

	public void OpenChangePFP()
	{
		changePFP.SetActive(true);
		chosenPFP = -1;
		if (content.childCount == 0)
		{
			for (int i = 0; i < AllCards.Instance.cards.Length; i++)
			{
				PFPButton p = Instantiate(PFPButton, content).GetComponent<PFPButton>();
				p.ID = i;
			}
            for (int i = 0; i < AllCards.Instance.heroes.Length; i++)
            {
                PFPButton p = Instantiate(PFPButton, content).GetComponent<PFPButton>();
                p.ID = i + AllCards.Instance.cards.Length;
            }
        }
	}

    public async void ConfirmChange()
	{
		UserAccounts.profilePicture = chosenPFP;
		await UserAccounts.Instance.SaveData();
        changePFP.SetActive(false);
		foreach (ProfileThumbnail p1 in FindObjectsByType<ProfileThumbnail>(FindObjectsSortMode.None)) p1.Refresh();
    }

}
