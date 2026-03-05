using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EyeForAnEye : Buff
{

    protected override IEnumerator OnCardHurt(Tuple<Damagable, Card, int, int> hurt)
    {
        if (hurt.Item1 == GameManager.Instance.GetTeamHero(team))
        {
            StartCoroutine(Glow());
            yield return GameManager.Instance.GetTeamHero(Card.GetOpponent(team)).ReceiveDamage((int)(hurt.Item3 / 2), null);
        }
        yield return base.OnCardHurt(hurt);
    }

}
