using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossPollination : Card
{

	protected override IEnumerator OnThisPlay()
	{
        yield return new WaitForSeconds(1);
        int id = AllCards.RandomFromTribe((Tribe.Flower, Tribe.Flower));
        FinalStats fs = new(id);
        fs.cost -= 1;
        yield return GameManager.Instance.GainHandCard(team, id, fs);
        id = AllCards.RandomFromTribe((Tribe.Fruit, Tribe.Fruit));
        FinalStats fs1 = new(id);
        fs1.cost -= 1;
        yield return GameManager.Instance.GainHandCard(team, id, fs1);
        yield return base.OnThisPlay();
	}

}
