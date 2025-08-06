using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frankentuar : Card
{

    protected override IEnumerator OnCardDeath(Tuple<Card, Card> died)
    {
        if (died.Item1.team == Team.Zombie)
        {
            yield return new WaitForSeconds(1);
            ChangeStats(1, 1);
        }
        yield return base.OnCardDeath(died);
    }

}
