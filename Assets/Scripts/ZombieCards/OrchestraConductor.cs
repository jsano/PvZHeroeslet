using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrchestraConductor : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		for (int row = 0; row < 2; row++)
		{
			for (int col = 0; col < 5; col++)
			{
				if (Tile.zombieTiles[row, col].HasRevealedPlanted()) Tile.zombieTiles[row, col].planted.ChangeStats(2, 0);
			}
		}
		yield return base.OnThisPlay();
	}

}
