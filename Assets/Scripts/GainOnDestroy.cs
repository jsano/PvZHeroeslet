using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GainOnDestroy : Card
{

	public GameObject toMake;

	protected override IEnumerator OnCardDeath(Tuple<Card, Card> died)
	{
		if (died.Item1 == this)
		{
			yield return new WaitForSeconds(1);
			yield return GameManager.Instance.GainHandCard(team, AllCards.NameToID(toMake.name));
		}
		yield return base.OnCardDeath(died);
	}

}
