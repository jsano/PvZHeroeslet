using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HibernatingBeary : Card
{

	protected override IEnumerator OnCardHurt(Tuple<Damagable, Card, int, int> hurt)
	{
		if (hurt.Item1 == this)
		{
			yield return Glow();
			ChangeStats(4, 0);
		}
		yield return base.OnCardHurt(hurt);
	}

}
