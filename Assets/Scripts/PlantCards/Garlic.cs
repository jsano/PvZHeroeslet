using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garlic : Card
{

	protected override IEnumerator OnCardHurt(Tuple<Damagable, Card, int, int> hurt)
	{
		if (hurt.Item1 == this && hurt.Item2.team == Team.Zombie && hurt.Item2.type == Type.Unit && hurt.Item2.col > 0)
		{
            yield return new WaitForSeconds(1);
			if (hurt.Item2.name.Contains("Vimpire")) hurt.Item2.Destroy();
			else hurt.Item2.Move(hurt.Item2.row, hurt.Item2.col - 1);
		}
		yield return base.OnCardHurt(hurt);
	}

}
