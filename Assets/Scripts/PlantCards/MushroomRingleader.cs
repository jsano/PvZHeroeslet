using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomRingleader : Card
{
    private int count = -2;

	protected override IEnumerator OnThisPlay()
	{
		for (int i = 0; i < Tile.ROWS; i++)
		{
			for (int j = 0; j < Tile.COLUMNS; j++)
			{
				count += Tile.GetTeamTiles(team)[i, j].HasRevealedPlanted() ? 2 : 0;
			}
		}
		if (count > 0)
		{
			yield return Glow();
			ChangeStats(count, 0);
		}
		yield return base.OnThisPlay();
	}

}
