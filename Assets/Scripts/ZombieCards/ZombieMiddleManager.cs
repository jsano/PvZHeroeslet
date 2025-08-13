using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieMiddleManager : Card
{

    protected override IEnumerator OnThisPlay()
    {
		yield return Glow();
		yield return GameManager.Instance.GainHandCard(team, AllCards.RandomFromTribe((Tribe.Professional, Tribe.Professional)));
        yield return base.OnThisPlay();
    }

    protected override IEnumerator OnCardHurt(Tuple<Damagable, Card, int, int> hurt)
	{
		if (hurt.Item1.GetComponent<Card>() != null && ((Card)hurt.Item1).tribes.Contains(Tribe.Professional))
		{
			yield return Glow();
			ChangeStats(1, 0);
		}
		yield return base.OnCardHurt(hurt);
	}

}
