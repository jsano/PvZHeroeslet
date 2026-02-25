using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LingeringRelief : Buff
{

    protected override int OnCardHealImmediate(Tuple<Card, int> healed)
    {
        if (healed.Item1.team == team) return 2;
        return 0;
    }

    protected override int OnHeroHealImmediate(Tuple<Hero, int> healed)
    {
        Debug.Log(healed.Item1 + "    " + healed.Item2);
        if (healed.Item1.team == team) return 2;
        return 0;
    }

}
