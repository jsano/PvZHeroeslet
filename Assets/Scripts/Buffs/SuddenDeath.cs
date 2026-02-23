using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SuddenDeath : Buff
{

    protected override IEnumerator OnTurnStart()
    {
        if (GameManager.Instance.turn >= 10)
        {
            StartCoroutine(GameManager.Instance.GetTeamHero(team).ReceiveDamage(10, null, true));
            yield return GameManager.Instance.GetTeamHero(Card.GetOpponent(team)).ReceiveDamage(10, null, true);
        }
        yield return base.OnTurnStart();
    }

}
