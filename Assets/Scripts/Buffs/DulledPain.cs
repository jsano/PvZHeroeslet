using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DulledPain : Buff
{

    private bool active;

    protected override int OnCardHurtImmediate(Tuple<Damagable, Card, int> hurt)
    {
        if (hurt.Item1 == GameManager.Instance.GetTeamHero(team) && active)
        {
            Debug.Log(hurt.Item3);
            active = false;
            return -(int)Mathf.Ceil(hurt.Item3 / 2f);
        }
        return base.OnCardHurtImmediate(hurt);
    }

    protected override IEnumerator OnTurnStart()
    {
        active = true;
        return base.OnTurnStart();
    }

}
