using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FirstBlood : Buff
{

    protected override int CardHurtModifiers(Tuple<Damagable, Card, int> hurt)
    {
        if (hurt.Item1 == GameManager.Instance.GetTeamHero(Card.GetOpponent(team)))
        {
            return Math.Max(0, 5 - GameManager.Instance.turn);
        }
        return base.CardHurtModifiers(hurt);
    }

}
