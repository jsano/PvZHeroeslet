using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixedNuts : Card
{

	protected override IEnumerator OnThisPlay()
	{
		for (int row = 0; row < Tile.ROWS; row++)
		{
			Card c = Tile.GetTeamTiles(team)[row, col].planted;
			if (c != null && c.teamUp)
			{
                yield return Glow();
                ChangeStats(2, 2);
				break;
			}
		}
		yield return base.OnThisPlay();
	}

}
