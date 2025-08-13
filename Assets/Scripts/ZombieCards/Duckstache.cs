using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duckstache : Card
{

    protected override IEnumerator OnThisPlay()
    {
		if (evolved)
		{
            yield return Glow();
            ChangeStats(3, 3);
        }
        yield return base.OnThisPlay();
    }

    protected override IEnumerator OnCardHurt(Tuple<Damagable, Card, int, int> hurt)
    {
        if (hurt.Item2 == this)
        {
            yield return Glow();
            int id = AllCards.RandomFromTribe((Tribe.Mustache, Tribe.Mustache));
            yield return GameManager.Instance.GainHandCard(team, id);
        }
        yield return base.OnCardHurt(hurt);
    }

}