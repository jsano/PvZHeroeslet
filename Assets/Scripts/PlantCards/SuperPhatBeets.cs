using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperPhatBeets : Card
{
    private int count = -1;

	protected override IEnumerator OnThisPlay()
	{
		for (int i = 0; i < 2; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				count += Tile.plantTiles[i, j].HasRevealedPlanted() ? 1 : 0;
                count += Tile.zombieTiles[i, j].HasRevealedPlanted() ? 1 : 0;
            }
		}
		if (count > 0)
		{
			yield return new WaitForSeconds(1);
			RaiseAttack(count);
			Heal(count, true);
		}
		yield return base.OnThisPlay();
	}

}
