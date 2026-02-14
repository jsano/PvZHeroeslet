using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BigLeagues : Buff
{

    protected override int OnCardHurtImmediate(Tuple<Damagable, Card, int> hurt)
    {
        if (hurt.Item2.team == team && hurt.Item3 < 2)
        {
            return 2 - hurt.Item3;
        }
        return base.OnCardHurtImmediate(hurt);
    }

}
