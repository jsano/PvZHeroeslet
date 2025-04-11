using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidRain : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		for (int j = 1; j < 4; j++) if (Tile.plantTiles[0, j].HasRevealedPlanted())
			{
				Tile.plantTiles[0, j].planted.RaiseAttack(-1);
                Tile.plantTiles[0, j].planted.Heal(-1, true);
            }
		yield return base.OnThisPlay();
	}

}
