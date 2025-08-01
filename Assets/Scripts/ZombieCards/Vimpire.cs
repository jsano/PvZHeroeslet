using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vimpire : Card
{

    protected override IEnumerator OnCardDeath(Tuple<Card, Card> died)
    {
        if (died.Item2 == this && died.Item1.team == Team.Plant)
        {
            yield return new WaitForSeconds(1);
            ChangeStats(2, 2);
        }
        yield return base.OnCardDeath(died);
    }

}
