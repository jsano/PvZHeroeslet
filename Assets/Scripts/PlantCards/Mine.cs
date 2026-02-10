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
			Damagable target = GetTargets(col)[0];
			if (target.GetComponent<Card>() != null)
			{
                yield return Glow();
                yield return AttackFX(target);
				yield return target.ReceiveDamage(deathDamage, this);
			}
		}
		yield return base.OnCardDeath(died);
    }

}
