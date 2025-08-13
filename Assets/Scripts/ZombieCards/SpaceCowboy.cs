using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceCowboy : Card
{

	protected override IEnumerator OnCardHurt(Tuple<Damagable, Card, int, int> hurt)
	{
		if (hurt.Item2 == this && hurt.Item1.GetComponent<Hero>() != null && col <= 3) 
		{
			yield return Glow();
			Move(row, col + 1);
        }
		yield return base.OnCardHurt(hurt);
	}

}
