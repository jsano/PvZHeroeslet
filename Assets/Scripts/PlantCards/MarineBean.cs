using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarineBean : Card
{

	protected override IEnumerator OnThisPlay()
	{
		int count = 0;
		yield return Glow();
		for (int row = 0; row < Tile.ROWS; row++)
		{
			for (int col = 0; col < Tile.COLUMNS; col++)
			{
				Card c = Tile.GetTeamTiles(team)[row, col].planted;
				if (c != null && c.amphibious && c != this) count += 1;
			}
		}
		ChangeStats(count, count);
		yield return base.OnThisPlay();
	}

}
