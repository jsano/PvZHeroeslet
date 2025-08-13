using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chomper : Card
{

	protected override IEnumerator OnThisPlay()
	{
		if (Tile.zombieTiles[0, col].HasRevealedPlanted() && Tile.zombieTiles[0, col].planted.atk <= 3)
		{
			yield return Glow();
			Tile.zombieTiles[0, col].planted.Destroy();
		}
		yield return base.OnThisPlay();
    }

}
