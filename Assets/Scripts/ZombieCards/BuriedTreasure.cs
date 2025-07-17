using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuriedTreasure : Card
{

	protected override IEnumerator Fusion(Card parent)
	{
		int id = AllCards.RandomFromCost(team, (7, 8, 9, 10, 11, 12));
		FinalStats fs = new(id);
		fs.cost -= 1;
		yield return GameManager.Instance.GainHandCard(team, id, fs); // TODO: LEGENDARIES
		yield return base.Fusion(parent);
    }

}
