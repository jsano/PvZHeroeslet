using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KetchupMechanic : Card
{
    private int count = 0;

	protected override IEnumerator OnThisPlay()
	{
		for (int j = 0; j < 5; j++)
		{
            count += Tile.zombieTiles[0, j].HasRevealedPlanted() ? 1 : 0;
        }
		if (count > 0)
		{
			yield return new WaitForSeconds(1);
			ChangeStats(count, count);
			yield return GameManager.Instance.plantHero.Heal(count);
		}
		yield return base.OnThisPlay();
	}

}
