using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoomShroom : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
        for (int i = 0; i < Tile.ROWS; i++) for (int j = 0; j < Tile.COLUMNS; j++)
        {
            if (Tile.opponentTiles[i, j].HasRevealedPlanted() && Tile.opponentTiles[i, j].planted.atk >= 4 && Tile.opponentTiles[i, j].planted.untrickable == 0) Tile.opponentTiles[i, j].planted.Destroy();
            if (Tile.playerTiles[i, j].HasRevealedPlanted() && Tile.playerTiles[i, j].planted.atk >= 4 && Tile.playerTiles[i, j].planted.untrickable == 0) Tile.playerTiles[i, j].planted.Destroy();
        }
		yield return base.OnThisPlay();
	}

}
