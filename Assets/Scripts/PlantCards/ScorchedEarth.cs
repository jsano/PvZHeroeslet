using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorchedEarth : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		for (int j = 1; j < 4; j++) if (Tile.zombieTiles[0, j].HasRevealedPlanted() && Tile.zombieTiles[0, j].planted.untrickable == 0)
			{
				Tile.zombieTiles[0, j].planted.ChangeStats(-1, -1);
            }
		yield return base.OnThisPlay();
	}

}
