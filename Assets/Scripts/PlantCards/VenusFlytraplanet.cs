using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VenusFlytraplanet : Card
{

	protected override IEnumerator OnCardHurt(Tuple<Damagable, Card, int, int> hurt)
	{
		if (!hurt.Item2.died && hurt.Item2.col == col && hurt.Item2.team == Team.Plant && hurt.Item2.type == Type.Unit) 
		{
            yield return new WaitForSeconds(1);
			yield return GameManager.Instance.plantHero.Heal(hurt.Item3);
        }
		yield return base.OnCardHurt(hurt);
	}

}
