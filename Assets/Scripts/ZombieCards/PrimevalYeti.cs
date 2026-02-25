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
            for (int r = 0; r < Tile.ROWS; r++) for (int j = 0; j < 5; j++)
			{
				if (Tile.GetTeamTiles(team)[r, j].HasRevealedPlanted()) Tile.GetTeamTiles(team)[r, j].planted.ChangeStats(2, 2);
			}
		}
		yield return base.OnThisPlay();
	}

}