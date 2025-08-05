using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighVoltageCurrant : Card
{

    protected override IEnumerator OnThisPlay()
    {
		yield return new WaitForSeconds(1);
		yield return GameManager.Instance.GainHandCard(team, AllCards.RandomFromTribe((Tribe.Berry, Tribe.Berry)));
        yield return base.OnThisPlay();
    }

    protected override IEnumerator OnCardHurt(Tuple<Damagable, Card, int, int> hurt)
	{
		if (hurt.Item2 != this && hurt.Item2.tribes.Contains(Tribe.Berry))
		{
			yield return new WaitForSeconds(1);
			ChangeStats(1, 0);
		}
		yield return base.OnCardHurt(hurt);
	}

}
