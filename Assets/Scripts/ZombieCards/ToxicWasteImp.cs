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

    protected override IEnumerator OnCardDeath(Card died)
    {
        if (died == this)
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

}
