using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pecanolith : Card
{

	protected override IEnumerator OnThisPlay()
	{
		for (int row = 0; row < 2; row++)
		{
			for (int col = 0; col < 5; col++)
			{
                if (Tile.plantTiles[row, col].HasRevealedPlanted()) Tile.plantTiles[row, col].planted.strengthHeart += 1;
                if (Tile.zombieTiles[row, col].HasRevealedPlanted()) Tile.zombieTiles[row, col].planted.strengthHeart += 1;
			}
		}
		yield return base.OnThisPlay();
	}

    protected override IEnumerator OnCardPlay(Card played)
    {
        played.strengthHeart += 1;
        yield return base.OnCardPlay(played);
    }

    protected override IEnumerator OnCardDeath(Tuple<Card, Card> died)
    {
        if (died.Item1 == this)
            for (int row = 0; row < 2; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    if (Tile.plantTiles[row, col].HasRevealedPlanted()) Tile.plantTiles[row, col].planted.strengthHeart += 1;
                    if (Tile.zombieTiles[row, col].HasRevealedPlanted()) Tile.zombieTiles[row, col].planted.strengthHeart += 1;
                }
            }
        yield return base.OnCardDeath(died);
    }

}
