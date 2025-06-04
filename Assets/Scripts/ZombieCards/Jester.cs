using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jester : Card
{

	protected override IEnumerator OnCardHurt(Tuple<Damagable, Card, int, int> hurt)
	{
		if (hurt.Item1.GetComponent<Card>() == this)
		{
            yield return AttackFX(Tile.plantHeroTiles[col]);
            yield return Tile.plantHeroTiles[col].ReceiveDamage(2, this);
		}
		yield return base.OnCardHurt(hurt);
	}

}
