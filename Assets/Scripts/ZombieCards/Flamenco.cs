using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamenco : Card
{

	protected override IEnumerator OnThisPlay()
	{
		int dmg = 0;
		for (int i = 0; i < 5; i++)
		{
			if (Tile.zombieTiles[0, i].planted != null && Tile.zombieTiles[0, i].planted.tribes.Contains(Tribe.Dancing)) dmg += 2;
		}
		yield return new WaitForSeconds(1);
		yield return GameManager.Instance.plantHero.ReceiveDamage(dmg, this);
		yield return base.OnThisPlay();
	}

}
