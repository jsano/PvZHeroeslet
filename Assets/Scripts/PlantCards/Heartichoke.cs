using System;
using System.Collections;
using UnityEngine;

public class Heartichoke : Card
{

    protected override IEnumerator OnCardHeal(Tuple<Card, int> healed)
	{
		if (healed.Item1.team == Team.Plant)
		{
            yield return Glow();
            yield return AttackFX(Tile.zombieHeroTiles[col]);
			yield return GameManager.Instance.zombieHero.ReceiveDamage(healed.Item2, this, bullseye > 0);
		}
		yield return null;
	}
	
	protected override IEnumerator OnHeroHeal(Tuple<Hero, int> healed)
    {
        if (healed.Item1.team == Team.Plant)
		{
            yield return Glow();
            yield return AttackFX(Tile.zombieHeroTiles[col]);
            yield return GameManager.Instance.zombieHero.ReceiveDamage(healed.Item2, this, bullseye > 0);
        }
		yield return null;
    }

}
