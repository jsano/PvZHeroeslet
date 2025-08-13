using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpCommander : Card
{

	protected override IEnumerator OnCardHurt(Tuple<Damagable, Card, int, int> hurt)
	{
		if (hurt.Item2.tribes.Contains(Tribe.Imp) && hurt.Item1.GetComponent<Hero>() != null) 
		{
            yield return Glow();
            yield return GameManager.Instance.DrawCard(team);
		}
		yield return base.OnCardHurt(hurt);
	}

}
