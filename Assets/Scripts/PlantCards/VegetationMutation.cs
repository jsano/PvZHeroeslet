using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VegetationMutation : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		for (int i = 0; i < 2; i++) for (int j = 0; j < 5; j++) if (Tile.plantTiles[i, j].HasRevealedPlanted() && (j == 0 || Tile.terrainTiles[j].planted != null))
				{
                    Tile.plantTiles[i, j].planted.ChangeStats(2, 2);
				}
		yield return base.OnThisPlay();
	}

}
