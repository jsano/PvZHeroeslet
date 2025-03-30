using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BriarRose : Card
{

	protected override IEnumerator OnCardHurt(Tuple<Damagable, Card> hurt)
	{
		if (hurt.Item1.GetComponent<Card>() != null && ((Card)hurt.Item1).tribes.Contains(Tribe.Flower) && !hurt.Item2.died)
		{
			GameManager.Instance.DisableHandCards();
			yield return new WaitForSeconds(1);
			hurt.Item2.Destroy();
		}
		yield return base.OnCardHurt(hurt);
	}

}
