using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeferredHope : Buff
{

    private int turns = 0;

    protected override void Start()
    {
        GameManager.Instance.GetTeamHero(team).ChangeStats(0, -15, true, true);
        base.Start();
    }

	protected override IEnumerator OnTurnStart()
    {
        turns += 1;
        if (turns == 2)
        {
            GameManager.Instance.GetTeamHero(team).ChangeStats(0, 30, true, true);
        }
        yield return base.OnTurnStart();
    }

}
