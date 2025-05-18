using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombotDroneEngineer : Card
{

	protected override IEnumerator OnCardHurt(Tuple<Damagable, Card, int, int> hurt)
	{
		if (hurt.Item2.tribes.Contains(Tribe.Science)) 
		{
            yield return new WaitForSeconds(1);
            hurt.Item2.ChangeStats(1, 0);
		}
		yield return base.OnCardHurt(hurt);
	}

}
