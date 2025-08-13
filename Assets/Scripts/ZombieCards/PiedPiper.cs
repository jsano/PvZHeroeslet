using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiedPiper : Card
{

	protected override IEnumerator OnThisPlay()
	{
		Card c = Tile.plantTiles[0, col].planted;
		Card c1 = Tile.plantTiles[1, col].planted;
		if (c != null || c1 != null)
		{
            yield return Glow();
            if (c != null) c.ChangeStats(-1, -1);
			if (c1 != null) c1.ChangeStats(-1, -1);
		}
		yield return base.OnThisPlay();
	}

}
