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
            yield return AttackFX(Tile.GetTeamHeroTiles(GetOpponent(team))[col]);
			yield return Tile.GetTeamHeroTiles(GetOpponent(team))[col].ReceiveDamage(6, this);
		}
		yield return base.OnCardDeath(died);
    }

}
