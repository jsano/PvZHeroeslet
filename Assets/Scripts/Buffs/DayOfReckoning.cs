using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayOfReckoning : Buff
{

    private HashSet<Card> markedCards = new();

    protected override IEnumerator OnCardHurt(Tuple<Damagable, Card, int, int> hurt)
    {
        Card c = hurt.Item1.GetComponent<Card>();
        if (c != null && c.team != team)
        {
            if (markedCards.Contains(c))
            {
                c.GetComponent<SpriteRenderer>().material.color = Color.white;
                markedCards.Remove(c);
            }
            else
            {
                c.GetComponent<SpriteRenderer>().material.color = Color.red;
                markedCards.Add(c);
            }
        }
        return base.OnCardHurt(hurt);
    }

    protected override int CardHurtModifiers(Tuple<Damagable, Card, int> hurt)
    {
        if (markedCards.Contains(hurt.Item1.GetComponent<Card>()))
        {
            return 3;
        }
        return base.CardHurtModifiers(hurt);
    }

}
