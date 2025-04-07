using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPummel : Card
{

	protected override IEnumerator OnThisPlay()
	{
        yield return new WaitForSeconds(1);

        int heroDmg = 0;
		for (int i = 3; i >= 1; i--)
		{
			if (Tile.zombieTiles[0, i].planted != null) yield return Tile.zombieTiles[0, i].planted.ReceiveDamage(2, this);
			else heroDmg += 2;
		}
		yield return GameManager.Instance.zombieHero.ReceiveDamage(heroDmg, this);

        yield return base.OnThisPlay();
	}

}
