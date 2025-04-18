using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorchedEarth : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		for (int j = 1; j < 4; j++) if (Tile.zombieTiles[0, j].HasRevealedPlanted())
			{
				Tile.zombieTiles[0, j].planted.RaiseAttack(-1);
                Tile.zombieTiles[0, j].planted.Heal(-1, true);
            }
		yield return base.OnThisPlay();
	}

}
