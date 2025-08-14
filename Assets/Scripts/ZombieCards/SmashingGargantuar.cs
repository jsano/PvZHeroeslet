using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashingGargantuar : Card
{

	protected override IEnumerator OnThisPlay()
	{
		for (int row = 0; row < 2; row++)
		{
			for (int col = 0; col < 5; col++)
			{
				if (Tile.zombieTiles[row, col].HasRevealedPlanted() && Tile.zombieTiles[row, col].planted.tribes.Contains(Tribe.Gargantuar))
                    Tile.zombieTiles[row, col].planted.frenzy += 1;
			}
		}
		yield return base.OnThisPlay();
	}

    protected override void OnCardPlayImmediate(Card played)
    {
        if (played.tribes.Contains(Tribe.Gargantuar)) played.frenzy += 1;
    }

    protected override IEnumerator OnCardDeath(Tuple<Card, Card> died)
    {
        if (died.Item1 == this)
            for (int row = 0; row < 2; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    if (Tile.zombieTiles[row, col].HasRevealedPlanted() && Tile.zombieTiles[row, col].planted.tribes.Contains(Tribe.Gargantuar))
                        Tile.zombieTiles[row, col].planted.frenzy -= 1;
                }
            }
        yield return base.OnCardDeath(died);
    }

    void OnDestroy()
    {
        if (died) return;
        for (int col = 0; col < 5; col++)
        {
            if (Tile.zombieTiles[0, col].HasRevealedPlanted() && Tile.zombieTiles[0, col].planted.tribes.Contains(Tribe.Gargantuar))
                Tile.zombieTiles[0, col].planted.frenzy -= 1;
        }
    }

}
