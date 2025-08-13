using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolBean : Card
{

	protected override IEnumerator OnThisPlay()
	{
        yield return Glow();
        for (int col = 0; col < 5; col++)
		{
			if (Tile.zombieTiles[0, col].planted != null && Tile.zombieTiles[0, col].planted.gravestone) Tile.zombieTiles[0, col].planted.Freeze();
		}
		yield return base.OnThisPlay();
	}

}
