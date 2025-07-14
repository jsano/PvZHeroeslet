using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astrocado : Card
{

	protected override IEnumerator OnCardDeath(Card died)
	{
		if (died == this)
		{
			yield return GameManager.Instance.GainHandCard(team, AllCards.NameToID("Astrocado Pit"));
		}
		yield return base.OnCardDeath(died);
	}

}
