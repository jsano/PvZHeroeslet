using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Haunting : Card
{

	protected override IEnumerator OnCardDeath(Card died)
	{
		if (died == this)
		{
			yield return GameManager.Instance.GainHandCard(team, AllCards.NameToID("Haunting Ghost"));
		}
		yield return base.OnCardDeath(died);
	}

}
