using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaidingRaptor : Card
{

    protected override IEnumerator OnCardHurt(Tuple<Damagable, Card, int, int> hurt)
    {
        if (hurt.Item2 == this && hurt.Item1 == GameManager.Instance.plantHero)
        {
            yield return new WaitForSeconds(1);
            int id = AllCards.RandomFromCost(team, (0, 1, 2));
            yield return GameManager.Instance.GainHandCard(team, id);
        }
        yield return base.OnCardHurt(hurt);
    }

    protected override IEnumerator OnCardDraw(Team t)
    {
		if (t == team)
		{
            yield return new WaitForSeconds(1);
            ChangeStats(2, 0);
        }
        yield return base.OnCardDraw(t);
    }

}