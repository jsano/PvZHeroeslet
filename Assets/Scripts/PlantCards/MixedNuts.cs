using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixedNuts : Card
{

	protected override IEnumerator OnThisPlay()
	{
		for (int row = 0; row < 2; row++)
		{
			Card c = Tile.plantTiles[row, col].planted;
			if (c != null && c.teamUp)
			{
				yield return new WaitForSeconds(1);
				RaiseAttack(2);
				Heal(2, true);
				break;
			}
		}
		yield return base.OnThisPlay();
	}

}
