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
				GameManager.Instance.DisableHandCards();
				yield return new WaitForSeconds(1);
				c.RaiseAttack(2);
				c.Heal(2, true);
				break;
			}
		}
		yield return base.OnThisPlay();
	}

}
