using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptainCucumber: Card
{

    protected override IEnumerator OnCardHurt(Tuple<Damagable, Card, int, int> hurt)
    {
        if (hurt.Item2 == this)
        {
            yield return Glow();
            int id = AllCards.NameToID("Reincarnation"); // TODO: LEGENDARIES
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