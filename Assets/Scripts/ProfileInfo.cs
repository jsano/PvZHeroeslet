using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileInfo : MonoBehaviour
{
	public Profile p;

    public Button exit;

    public void Show(string playerID, string username, string score, string rank)
	{
        if (isActiveAndEnabled) return;
		transform.parent.gameObject.SetActive(true);
		p.LoadProfileThumbnail(playerID, username, score, rank);
		
    }

}
