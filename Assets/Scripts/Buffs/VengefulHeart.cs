using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VengefulHeart : Buff
{

    protected override IEnumerator OnCardHurt(Tuple<Damagable, Card, int, int> hurt)
    {
        Card c = hurt.Item1.GetComponent<Card>();
        if (c != null && c.team == team && !c.died)
        {
            StartCoroutine(Glow());
            yield return c.Heal(1000);
        }
        yield return base.OnCardHurt(hurt);
    }

}
