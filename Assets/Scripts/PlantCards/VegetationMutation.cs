using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VegetationMutation : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		for (int i = 0; i < Tile.ROWS; i++) for (int j = 0; j < Tile.COLUMNS; j++) if (Tile.GetTeamTiles(team)[i, j].HasRevealedPlanted() && (j == 0 || Tile.terrainTiles[j].planted != null))
				{
                    Tile.GetTeamTiles(team)[i, j].planted.ChangeStats(2, 2);
				}
		yield return base.OnThisPlay();
	}

}
