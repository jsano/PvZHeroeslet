using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KiteFlyer : Card
{

	protected override IEnumerator OnCardHurt(Tuple<Damagable, Card, int, int> hurt)
	{
		if (hurt.Item2 == this) 
		{
            yield return Glow();
			yield return GameManager.Instance.DrawCard(team);
		}
		yield return base.OnCardHurt(hurt);
	}

}
