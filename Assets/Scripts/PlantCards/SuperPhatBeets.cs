using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperPhatBeets : Card
{
    private int count = -1;

	protected override IEnumerator OnThisPlay()
	{
		for (int i = 0; i < Tile.ROWS; i++)
		{
			for (int j = 0; j < Tile.COLUMNS; j++)
			{
				count += Tile.playerTiles[i, j].HasRevealedPlanted() ? 1 : 0;
                count += Tile.opponentTiles[i, j].HasRevealedPlanted() ? 1 : 0;
            }
		}
		if (count > 0)
		{
			yield return Glow();
			ChangeStats(count, count);
		}
		yield return base.OnThisPlay();
	}

}
