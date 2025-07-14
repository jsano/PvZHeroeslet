using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VenusFlytrap : Card
{

	protected override IEnumerator OnCardHurt(Tuple<Damagable, Card, int, int> hurt)
	{
		if (hurt.Item2 == this)
		{
			yield return new WaitForSeconds(1);
			yield return GameManager.Instance.plantHero.Heal(hurt.Item3);
		}
		yield return base.OnCardHurt(hurt);
	}

}
