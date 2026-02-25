using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormFront : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		for (int i = 0; i < Tile.ROWS; i++) for (int j = 0; j < Tile.COLUMNS; j++) if (Tile.GetTeamTiles(team)[i, j].HasRevealedPlanted())
				{
                    Tile.GetTeamTiles(team)[i, j].planted.ChangeStats(1, 1);
				}
		yield return base.OnThisPlay();
	}

}
