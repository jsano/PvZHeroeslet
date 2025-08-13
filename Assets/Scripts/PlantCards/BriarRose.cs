using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BriarRose : Card
{

	protected override IEnumerator OnCardHurt(Tuple<Damagable, Card, int, int> hurt)
	{
		if (hurt.Item1.GetComponent<Card>() != null && ((Card)hurt.Item1).tribes.Contains(Tribe.Flower) && hurt.Item2 != null && !hurt.Item2.died)
		{
			yield return Glow();
			hurt.Item2.Destroy();
		}
		yield return base.OnCardHurt(hurt);
	}

}
