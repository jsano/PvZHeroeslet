using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrSpacetime : Card
{

    protected override IEnumerator OnCardHurt(Tuple<Damagable, Card, int, int> hurt)
    {
        if (hurt.Item2 == this && hurt.Item1.GetComponent<Hero>() != null)
        {
            yield return new WaitForSeconds(1);
            int id = AllCards.RandomFromCost(team, (0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12)); // TODO: GALACTIC
            yield return GameManager.Instance.GainHandCard(team, id);
        }
        yield return base.OnCardHurt(hurt);
    }

    protected override IEnumerator OnCardDraw(Team t)
    {
		if (t == team)
		{
			HandCard c = GameManager.Instance.GetHandCards()[0];
			if (c.conjured) c.ChangeCost(-1);
		}
        return base.OnCardDraw(t);
    }

}