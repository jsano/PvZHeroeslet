using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombats : Card
{

	protected override IEnumerator OnCardHurt(Tuple<Damagable, Card, int, int> hurt)
	{
		if (hurt.Item2 == this && hurt.Item1.GetComponent<Card>() != null && hurt.Item1.GetComponent<Card>().team == Team.Plant)
		{
            yield return Glow();
            yield return GameManager.Instance.DrawCard(team);
		}
		yield return base.OnCardHurt(hurt);
	}

}
