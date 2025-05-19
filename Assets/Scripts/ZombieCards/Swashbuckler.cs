using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swashbuckler : Card
{

	protected override IEnumerator OnCardHurt(Tuple<Damagable, Card, int, int> hurt)
	{
		if (hurt.Item2.tribes.Contains(Tribe.Pirate) && hurt.Item1.GetComponent<Hero>() != null) 
		{
            yield return new WaitForSeconds(1);
            hurt.Item2.ChangeStats(1, 1);
		}
		yield return base.OnCardHurt(hurt);
	}

}
