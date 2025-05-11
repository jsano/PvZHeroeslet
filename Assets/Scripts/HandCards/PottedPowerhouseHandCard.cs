using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class PottedPowerhouseHandCard : HandCard
{

    protected override IEnumerator OnCardStatsGained(Card gained)
    {
        ChangeAttack(1);
        ChangeHP(1);
        yield return base.OnCardStatsGained(gained);
    }

}
