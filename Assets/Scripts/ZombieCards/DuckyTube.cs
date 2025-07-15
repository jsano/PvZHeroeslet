using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckyTube : Card
{

	protected override IEnumerator OnCardHurt(Tuple<Damagable, Card, int, int> hurt)
	{
		if (hurt.Item2 == this && hurt.Item1.GetComponent<Hero>() != null) 
		{
			yield return new WaitForSeconds(1);
			ChangeStats(1, 1);
        }
		yield return base.OnCardHurt(hurt);
	}

}
