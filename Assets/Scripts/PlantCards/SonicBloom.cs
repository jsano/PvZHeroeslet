using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonicBloom : Card
{
    
	private int count = 0;

	protected override IEnumerator OnThisPlay()
	{
		for (int i = 0; i < 2; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				count += Tile.plantTiles[i, j].HasRevealedPlanted() ? 1 : 0;
            }
		}
        yield return Glow();
        yield return AttackFX(Tile.zombieHeroTiles[col]);
        yield return GameManager.Instance.zombieHero.ReceiveDamage(count, this, bullseye > 0);
		yield return base.OnThisPlay();
	}

}
