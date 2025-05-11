using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorNut : Card
{

	protected override IEnumerator OnCardHurt(Tuple<Damagable, Card, int, int> hurt)
	{
		if (hurt.Item1.GetComponent<Card>() != null && ((Card)hurt.Item1).tribes.Contains(Tribe.Nut))
		{
			yield return new WaitForSeconds(1);
			yield return Tile.zombieHeroTiles[col].ReceiveDamage(2, this);
		}
		yield return base.OnCardHurt(hurt);
	}

}
