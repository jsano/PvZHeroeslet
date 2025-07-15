using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CroMagnolia : Card
{

	protected override IEnumerator OnThisPlay()
	{
		if (evolved)
		{
			yield return new WaitForSeconds(1);
			for (int i = -1; i <= 1; i++)
			{
				if (col + i < 0 || col + i > 4) continue;
				for (int j = 0; j < 2; j++) if (Tile.plantTiles[j, col + i].planted != null) Tile.plantTiles[j, col + i].planted.ChangeStats(2, 0);
			}
		}
		yield return base.OnThisPlay();
	}

}