using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarineBean : Card
{

	protected override IEnumerator OnThisPlay()
	{
		int count = 0;
		yield return Glow();
		for (int row = 0; row < 2; row++)
		{
			for (int col = 0; col < 5; col++)
			{
				Card c = Tile.plantTiles[row, col].planted;
				if (c != null && c.amphibious && c != this) count += 1;
			}
		}
		ChangeStats(count, count);
		yield return base.OnThisPlay();
	}

}
