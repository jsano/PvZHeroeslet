using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class PottedPowerhouseHandCard : HandCard
{

    protected override IEnumerator OnCardStatsChanged(Card changed)
    {
        if (changed.team == Card.Team.Plant)
        {
            ChangeAttack(1);
            ChangeHP(1);
        }
        yield return base.OnCardStatsChanged(changed);
    }

}
