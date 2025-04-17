using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SteelMagnolia : Card
{

	protected override IEnumerator OnThisPlay()
	{
        yield return new WaitForSeconds(1);

		for (int i = 1; i >= -1; i--)
		{
            if (col + i < 0 || col + i > 4) continue;
            for (int r = 0; r < 2; r++)
            {
                if (Tile.plantTiles[r, col + i].HasRevealedPlanted()) Tile.plantTiles[r, col + i].planted.Heal(2, true);
            }
		}

		yield return base.OnThisPlay();
	}

}