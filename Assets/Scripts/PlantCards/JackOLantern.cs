using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackOLantern : Card
{

	protected override IEnumerator OnCardHurt(Tuple<Damagable, Card, int, int> hurt)
	{
		if (hurt.Item2 == this && hurt.Item1.GetComponent<Hero>() != null && hurt.Item1.GetComponent<Hero>().team != team) 
		{
			yield return new WaitForSeconds(1);
			ChangeStats(1, 0);
        }
		yield return base.OnCardHurt(hurt);
	}

}
