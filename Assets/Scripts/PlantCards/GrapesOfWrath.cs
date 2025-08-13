using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapesOfWrath : Card
{

	protected override IEnumerator OnCardDeath(Tuple<Card, Card> died)
	{
		if (died.Item1 == this)
		{
            yield return Glow();
            yield return AttackFX(Tile.zombieHeroTiles[col]);
			yield return Tile.zombieHeroTiles[col].ReceiveDamage(6, this);
		}
		yield return base.OnCardDeath(died);
    }

}
