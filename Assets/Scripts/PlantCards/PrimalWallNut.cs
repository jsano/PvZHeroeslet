using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimalWallNut : Card
{

    protected override IEnumerator OnThisPlay()
    {
        yield return Glow();
        yield return GameManager.Instance.GainHandCard(team, AllCards.RandomFromCost(team, (4, 5, 6, 7, 8, 9, 10, 11, 12)));
        yield return base.OnThisPlay();
    }

}
