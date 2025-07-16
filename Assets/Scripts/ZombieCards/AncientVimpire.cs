using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AncientVimpire : Card
{

    protected override IEnumerator OnCardDeath(Tuple<Card, Card> died)
    {
        if (died.Item1.team == Team.Plant && died.Item2 != null && died.Item2.frenzy > 0)
        {
            yield return new WaitForSeconds(1);
            died.Item2.ChangeStats(2, 2);
        }
        yield return base.OnCardDeath(died);
    }

}
