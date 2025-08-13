using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimevalYeti : Card
{

	protected override IEnumerator OnThisPlay()
	{
		if (evolved)
		{
            yield return Glow();
            for (int j = 0; j < 5; j++)
			{
				if (Tile.zombieTiles[0, j].HasRevealedPlanted()) Tile.zombieTiles[0, j].planted.ChangeStats(2, 2);
			}
		}
		yield return base.OnThisPlay();
	}

}