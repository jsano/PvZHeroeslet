using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicWasteImp : Card
{

	protected override IEnumerator OnThisPlay()
	{
		for (int row = 0; row < 2; row++)
		{
			for (int col = 0; col < 5; col++)
			{
				if (Tile.zombieTiles[row, col].HasRevealedPlanted() && Tile.zombieTiles[row, col].planted.tribes.Contains(Tribe.Imp))
                    Tile.zombieTiles[row, col].planted.deadly += 1;
			}
		}
		yield return base.OnThisPlay();
	}

    protected override IEnumerator OnCardPlay(Card played)
    {
        if (played.tribes.Contains(Tribe.Imp)) played.deadly += 1;
        yield return base.OnCardPlay(played);
    }

    protected override IEnumerator OnCardDeath(Tuple<Card, Card> died)
    {
        if (died.Item1 == this)
            for (int row = 0; row < 2; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    if (Tile.zombieTiles[row, col].HasRevealedPlanted() && Tile.zombieTiles[row, col].planted.tribes.Contains(Tribe.Imp))
                        Tile.zombieTiles[row, col].planted.deadly -= 1;
                }
            }
        yield return base.OnCardDeath(died);
    }

    void OnDestroy()
    {
        if (died) return;
        for (int col = 0; col < 5; col++)
        {
            if (Tile.zombieTiles[0, col].HasRevealedPlanted() && Tile.zombieTiles[0, col].planted.tribes.Contains(Tribe.Imp))
                Tile.zombieTiles[0, col].planted.deadly -= 1;
        }
    }

}
