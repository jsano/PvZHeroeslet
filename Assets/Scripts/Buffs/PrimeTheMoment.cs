using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrimeTheMoment : Buff
{

    private int prevTurn = -1;

    protected override IEnumerator OnTurnStart()
    {
        if (GameManager.Instance.turn != prevTurn)
        {
            StartCoroutine(Glow());
            GameManager.Instance.TriggerEvent("OnTurnStart", null);
        }
        prevTurn = GameManager.Instance.turn;
        yield return base.OnTurnStart();
    }

}
