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
			GameManager.Instance.DisableHandCards();
			yield return new WaitForSeconds(1);
			if (c != null)
			{
				c.RaiseAttack(-1);
				c.Heal(-1, true);
			}
			if (c1 != null)
			{
				c1.RaiseAttack(-1);
				c1.Heal(-1, true);
			}
		}
		yield return base.OnThisPlay();
	}

}
