using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoomShroom : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
        for (int i = 0; i < 2; i++) for (int j = 0; j < 5; j++)
        {
            if (Tile.zombieTiles[i, j].HasRevealedPlanted() && Tile.zombieTiles[i, j].planted.atk >= 4 && Tile.zombieTiles[i, j].planted.untrickable == 0) Tile.zombieTiles[i, j].planted.Destroy();
            if (Tile.plantTiles[i, j].HasRevealedPlanted() && Tile.plantTiles[i, j].planted.atk >= 4) Tile.plantTiles[i, j].planted.Destroy();
        }
		yield return base.OnThisPlay();
	}

}
