using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MercyKill : Buff
{

    protected override IEnumerator OnCardHurt(Tuple<Damagable, Card, int, int> hurt)
    {
        Card c = hurt.Item1.GetComponent<Card>();
        if (c != null && c.team != team && c.HP == 1 && c.isDamaged())
        {
            c.Destroy();
        }
        yield return base.OnCardHurt(hurt);
    }

}
