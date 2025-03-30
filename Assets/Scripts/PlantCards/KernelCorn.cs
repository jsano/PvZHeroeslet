using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KernelCorn : Card
{

	protected override IEnumerator OnThisPlay()
	{
		GameManager.Instance.DisableHandCards();
		yield return new WaitForSeconds(1);
		for (int col = 0; col < 5; col++)
		{
			if (Tile.zombieTiles[0, col].planted != null) StartCoroutine(Tile.zombieTiles[0, col].planted.ReceiveDamage(4, this));
		}

		yield return base.OnThisPlay();
	}

}
