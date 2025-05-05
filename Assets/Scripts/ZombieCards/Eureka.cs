using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eureka : Card
{

    protected override IEnumerator OnThisPlay()
    {
        yield return new WaitForSeconds(1);
        for (int i = 0; i < 3; i++)
        {
            yield return GameManager.Instance.GainHandCard(team, AllCards.RandomFromCost(team, (0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13)));
        }
        yield return base.OnThisPlay();
    }

}
