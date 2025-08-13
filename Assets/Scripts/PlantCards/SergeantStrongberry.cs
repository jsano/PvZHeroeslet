using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SergeantStrongberry : Card
{

	protected override IEnumerator OnCardHurt(Tuple<Damagable, Card, int, int> hurt)
	{
		if ((hurt.Item1.GetComponent<Card>() != null && ((Card)hurt.Item1).team == Team.Zombie || hurt.Item1.GetComponent<Hero>() != null && ((Hero)hurt.Item1).team == Team.Zombie)
			&& hurt.Item2.tribes.Contains(Tribe.Berry) && hurt.Item2 != this) 
		{
            yield return Glow();
            yield return AttackFX(hurt.Item1);
            yield return hurt.Item1.ReceiveDamage(2, this);
		}
		yield return base.OnCardHurt(hurt);
	}

}
