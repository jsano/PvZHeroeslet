using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PricklyPear : Card
{

	protected override IEnumerator OnCardHurt(Tuple<Damagable, Card, int, int> hurt)
	{
		if (hurt.Item1 == this && Tile.zombieTiles[0, col].planted != null)
		{
            yield return Glow();
            yield return AttackFX(Tile.zombieTiles[0, col].planted);
            yield return Tile.zombieTiles[0, col].planted.ReceiveDamage(4, this);
		}
		yield return base.OnCardHurt(hurt);
	}

}
