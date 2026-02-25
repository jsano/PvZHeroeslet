using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ErodingDefense: Buff
{

    protected override IEnumerator OnTurnStart()
    {
        GameManager.Instance.GetTeamHero(Card.GetOpponent(team)).StealBlock(1);
        yield return base.OnTurnStart();
    }

}
