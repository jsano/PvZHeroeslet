using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smackadamia : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return Glow();
		for (int row = 0; row < Tile.ROWS; row++)
		{
			for (int col = 0; col < Tile.COLUMNS; col++)
			{
				Card c = Tile.GetTeamTiles(team)[row, col].planted;
				if (c != null && c.tribes.Contains(Tribe.Nut))
				{
					c.ChangeStats(0, 2);
				}
			}
		}
		yield return base.OnThisPlay();
	}

}
