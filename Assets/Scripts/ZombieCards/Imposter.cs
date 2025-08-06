using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Imposter : Card
{

	protected override IEnumerator OnCardDeath(Tuple<Card, Card> died)
	{
		if (died.Item1 == this)
		{
			yield return new WaitForSeconds(1);
			yield return GameManager.Instance.GainHandCard(team, AllCards.RandomTribeOfCost(Tribe.Imp, 1, true)); // I'm not doing mustache...
		}
		yield return base.OnCardDeath(died);
	}

}
