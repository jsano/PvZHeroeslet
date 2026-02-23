using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColdSweat : Buff
{

    protected override int OnCardHurtImmediate(Tuple<Damagable, Card, int> hurt)
    {
        if (hurt.Item1 == GameManager.Instance.GetTeamHero(team))
        {
            int gold = (int)(team == GameManager.Instance.team ? GameManager.Instance.remaining : GameManager.Instance.opponentRemaining);
            StartCoroutine(GameManager.Instance.UpdateRemaining(-gold, team, false));
            return -gold;
        }
        return base.OnCardHurtImmediate(hurt);
    }

}
