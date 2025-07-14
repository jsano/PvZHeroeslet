using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : Card
{

	public int deathDamage;

	protected override IEnumerator OnCardDeath(Tuple<Card, Card> died)
	{
		if (died.Item1 == this)
		{
			if (Tile.zombieTiles[0, col].planted != null)
			{
				yield return AttackFX(Tile.zombieTiles[0, col].planted);
				yield return Tile.zombieTiles[0, col].planted.ReceiveDamage(deathDamage, this);
			}
		}
		yield return base.OnCardDeath(died);
    }

}
