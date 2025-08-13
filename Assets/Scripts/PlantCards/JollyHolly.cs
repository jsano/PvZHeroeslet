using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JollyHolly : Card
{

	protected override IEnumerator OnThisPlay()
	{
        yield return Glow();

        for (int i = 1; i >= -1; i -= 2)
		{
            if (col + i < 0 || col + i > 4) continue;
            if (Tile.zombieTiles[0, col + i].HasRevealedPlanted()) Tile.zombieTiles[0, col + i].planted.Freeze();
		}

		yield return base.OnThisPlay();
	}

}