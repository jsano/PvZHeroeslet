using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moonwalker : Card
{

	protected override IEnumerator OnThisPlay()
	{
		if (col == 0 || Tile.terrainTiles[col].planted != null)
		{
			yield return new WaitForSeconds(1);
			ChangeStats(2, 2);
		}
		yield return base.OnThisPlay();
	}

}
