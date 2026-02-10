using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoodOfVigor : Buff
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        if (team == GameManager.Instance.team)
        {
            GameManager.Instance.playerUnitPermanentDiscount += 0.5f;
            GameManager.Instance.playerTrickPermanentDiscount += 0.5f;
        }
        else
        {
            GameManager.Instance.opponentUnitPermanentDiscount += 0.5f;
            GameManager.Instance.opponentTrickPermanentDiscount += 0.5f;
        }
        if (team == GameManager.Instance.team) foreach (HandCard hc in GameManager.Instance.GetHandCards()) hc.ChangeCost(0);
        base.Start();
    }

}
