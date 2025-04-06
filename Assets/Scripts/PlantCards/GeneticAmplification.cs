using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAmplification : Card
{

	protected override IEnumerator OnThisPlay()
	{
        yield return new WaitForSeconds(1);

        int c = AllCards.RandomFromCost(team, (2, 2), true);
        HandCard.FinalStats fs = HandCard.MakeDefaultFS(c);
        fs.atk += 2;
        fs.hp += 1;
        fs.abilities += " - amphibious - teamUp";
        GameManager.Instance.GainHandCard(team, c, fs);

        yield return base.OnThisPlay();
	}

}
