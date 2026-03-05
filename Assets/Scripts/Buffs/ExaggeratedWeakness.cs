using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExaggeratedWeakness : Buff
{

    protected override IEnumerator OnCardStatsChanged(Tuple<Card, int, int> changed)
    {
        if (changed.Item1.team != team)
        {
            StartCoroutine(Glow());
            changed.Item1.ChangeStats(changed.Item2 < 0 ? -1 : 0, changed.Item3 < 0 ? -1 : 0, false, true);
        }
        return base.OnCardStatsChanged(changed);
    }

}
