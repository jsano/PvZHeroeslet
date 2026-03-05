using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpikedShell : Buff
{

    protected override IEnumerator OnCardHurt(Tuple<Damagable, Card, int, int> hurt)
    {
        if (hurt.Item1 == GameManager.Instance.GetTeamHero(team))
        {
            if (hurt.Item2 != null)
            {
                StartCoroutine(Glow());
                yield return hurt.Item2.ReceiveDamage(1, null);
            }
        }
        yield return base.OnCardHurt(hurt);
    }

}
