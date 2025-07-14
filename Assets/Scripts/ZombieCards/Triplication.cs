using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triplication : Card
{

    protected override IEnumerator OnThisPlay()
    {
        yield return new WaitForSeconds(1);
        yield return GameManager.Instance.GainHandCard(team, AllCards.RandomFromTribe((Tribe.Imp, Tribe.Imp)));
        yield return GameManager.Instance.GainHandCard(team, AllCards.RandomFromCost(team, (0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12), true));
        yield return GameManager.Instance.GainHandCard(team, AllCards.RandomFromTribe((Tribe.Gargantuar, Tribe.Gargantuar)));
        yield return base.OnThisPlay();
    }

}
