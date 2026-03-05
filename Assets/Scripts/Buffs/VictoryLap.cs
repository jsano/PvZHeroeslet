using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VictoryLap : Buff
{

    protected override IEnumerator OnTurnEnd()
    {
        int mine = GameManager.Instance.GetTeamHero(team).HP;
        int opponent = GameManager.Instance.GetTeamHero(Card.GetOpponent(team)).HP;
        if (mine > opponent)
        {
            StartCoroutine(Glow());
            yield return GameManager.Instance.DrawCard(team);
        }
        yield return base.OnTurnEnd();
    }

}
