using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PricklyPear : Card
{

	protected override IEnumerator OnCardHurt(Tuple<Damagable, Card, int, int> hurt)
	{
		Damagable target = GetTargets(col)[0];
		if (hurt.Item1 == this && target.GetComponent<Card>() != null)
		{
            yield return Glow();
            yield return AttackFX(target);
            yield return target.ReceiveDamage(4, this);
		}
		yield return base.OnCardHurt(hurt);
	}

}
