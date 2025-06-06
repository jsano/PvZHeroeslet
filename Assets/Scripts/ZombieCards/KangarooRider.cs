using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KangarooRider : Card
{

	protected override IEnumerator OnCardHurt(Tuple<Damagable, Card, int, int> hurt)
	{
		if (hurt.Item1 == this && !died)
		{
			yield return new WaitForSeconds(1);
			Bounce();
		}
		yield return base.OnCardHurt(hurt);
	}

}
