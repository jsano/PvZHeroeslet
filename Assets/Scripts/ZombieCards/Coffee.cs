using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coffee : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		for (int row = 0; row < 2; row++)
		{
			for (int col = 0; col < 5; col++)
			{
				if (Tile.zombieTiles[row, col].HasRevealedPlanted())
				{
					Tile.zombieTiles[row, col].planted.ChangeStats(1, 1);
					Tile.zombieTiles[row, col].planted.frenzy += 1;
                }
			}
		}
		yield return base.OnThisPlay();
	}

}
