using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupernovaGargantuar : Card
{

    private List<Card> toDestroy = new();

    protected override IEnumerator OnCardDeath(Tuple<Card, Card> died)
    {
        if (died.Item1.team == Team.Plant && (died.Item2 != null && died.Item2.tribes.Contains(Tribe.Gargantuar)) && !toDestroy.Contains(died.Item1))
        {
            yield return Glow();
            for (int i = 0; i < 2; i++) for (int j = 0; j < 5; j++)
            {
                var c = Tile.plantTiles[i, j].planted;
                if (c != null && AllCards.InstanceToPrefab(c) == AllCards.InstanceToPrefab(died.Item1))
                {
                    toDestroy.Add(c);
                    c.Destroy();
                }
            }
        }
        yield return base.OnCardDeath(died);
    }

}
