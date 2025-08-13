using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeHeadedChomper : Card
{

	protected override IEnumerator OnTurnEnd()
	{
		yield return Glow();
        for (int i = -1; i <= 1; i++)
        {
            if (col + i < 0 || col + i >= 5) continue;
            if (Tile.zombieTiles[0, col + i].HasRevealedPlanted()) Tile.zombieTiles[0, col + i].planted.Destroy();
        }
        yield return base.OnTurnEnd();
    }

}
