using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class ReincarnationHandCard : HandCard
{

    protected override IEnumerator OnTurnEnd()
    {
        ID = AllCards.RandomFromCost(Card.Team.Plant, (0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10), true);
        FinalStats fs = new(ID);
        fs.atk += 1;
        fs.hp += 1;
        OverrideFS(fs);
        Start();
        yield return base.OnTurnEnd();
    }

}
