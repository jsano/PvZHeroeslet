using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomRingleader : Card
{
    private int count = -2;

	protected override IEnumerator OnThisPlay()
	{
		for (int i = 0; i < 2;  i++)
		{
			for (int j = 0; j < 5; j++)
			{
				count += Tile.plantTiles[i, j].HasRevealedPlanted() ? 2 : 0;
			}
		}
		if (count > 0)
		{
			yield return new WaitForSeconds(1);
			RaiseAttack(count);
		}
		yield return base.OnThisPlay();
	}

}
