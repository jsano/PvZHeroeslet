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
            yield return Glow();
            yield return AttackFX(Tile.GetTeamHeroTiles(GetOpponent(team))[col]);
            yield return Tile.GetTeamHeroTiles(GetOpponent(team))[col].ReceiveDamage(2, this);
		}
		yield return base.OnCardHurt(hurt);
	}

}
