using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedStinger : Card
{

	protected override IEnumerator OnThisPlay()
	{
		if (Tile.plantTiles[1, col].planted != null && row == 0)
		{
			yield return new WaitForSeconds(1);
			SetStats(7, 3);
		}
		yield return base.OnThisPlay();
	}

}
