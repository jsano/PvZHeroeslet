using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimalPeashooter : Card
{

	protected override IEnumerator OnCardHurt(Tuple<Damagable, Card, int, int> hurt)
	{
		Card target = hurt.Item1.GetComponent<Card>();
		if (hurt.Item2 == this && target != null && target.team == Team.Zombie && !target.died) 
		{
            yield return new WaitForSeconds(1);
			target.Bounce();
		}
		yield return base.OnCardHurt(hurt);
	}

}
