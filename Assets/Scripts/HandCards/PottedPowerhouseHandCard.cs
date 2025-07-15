using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class PottedPowerhouseHandCard : HandCard
{

    protected override IEnumerator OnCardStatsChanged(Tuple<Card, int, int> changed)
    {
        if (changed.Item1.team == Card.Team.Plant && (changed.Item2 > 0 || changed.Item3 > 0))
        {
            ChangeAttack(1);
            ChangeHP(1);
        }
        yield return base.OnCardStatsChanged(changed);
    }

}
