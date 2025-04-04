using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldcrusherViking : Card
{

	protected override IEnumerator OnCardHurt(Tuple<Damagable, Card, int, int> hurt)
	{
		if (hurt.Item2 == this && hurt.Item1 == GameManager.Instance.plantHero)
		{
			yield return new WaitForSeconds(1);
			GameManager.Instance.plantHero.StealBlock(10);
		}
		yield return base.OnCardHurt(hurt);
	}

}
