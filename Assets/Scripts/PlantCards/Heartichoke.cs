using System;
using System.Collections;
using UnityEngine;

public class Heartichoke : Card
{

    protected override IEnumerator OnCardHeal(Tuple<Card, int> healed)
	{
		if (healed.Item1.team == team)
		{
            yield return Glow();
            yield return AttackFX(Tile.GetTeamHeroTiles(GetOpponent(team))[col]);
			yield return GameManager.Instance.GetTeamHero(GetOpponent(team)).ReceiveDamage(healed.Item2, this, bullseye > 0);
		}
		yield return null;
	}
	
	protected override IEnumerator OnHeroHeal(Tuple<Hero, int> healed)
    {
        if (healed.Item1.team == Team.A)
		{
            yield return Glow();
            yield return AttackFX(Tile.GetTeamHeroTiles(GetOpponent(team))[col]);
            yield return GameManager.Instance.GetTeamHero(GetOpponent(team)).ReceiveDamage(healed.Item2, this, bullseye > 0);
        }
		yield return null;
    }

}
