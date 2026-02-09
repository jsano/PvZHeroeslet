using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoodOfPassivity : Buff
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        if (team == Card.Team.A)
        {
            GameManager.Instance.plantTrickPermanentDiscount += 1;
        }
        else
        {
            GameManager.Instance.zombieTrickPermanentDiscount += 1f;
        }
        if (team == GameManager.Instance.team) foreach (HandCard hc in GameManager.Instance.GetHandCards()) hc.ChangeCost(0);
        base.Start();
    }

}
