using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buffshroom : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return Glow();
		for (int row = 0; row < 2; row++)
		{
			for (int col = 0; col < 5; col++)
			{
				Card c = Tile.plantTiles[row, col].planted;
				if (c != null && c.tribes.Contains(Tribe.Mushroom))
				{
					c.ChangeStats(1, 1);
				}
			}
		}
		yield return base.OnThisPlay();
	}

}
