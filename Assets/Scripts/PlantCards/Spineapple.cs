using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spineapple : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return Glow();
		for (int row = 0; row < 2;  row++)
		{
			for (int col = 0; col < 5; col++)
			{
				Card c = Tile.plantTiles[row, col].planted;
				if (c != null && c.atk <= 0)
				{
					c.ChangeStats(2, 0);
				}
			}
		}
		yield return base.OnThisPlay();
	}

}
