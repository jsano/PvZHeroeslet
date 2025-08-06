using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombologyTeacher : Card
{

	protected override IEnumerator OnThisPlay()
	{
		if (GameManager.Instance.team == team) foreach (HandCard hc in GameManager.Instance.GetHandCards()) if (hc.orig.type == Type.Trick) hc.ChangeCost(-1);
		yield return base.OnThisPlay();
	}

    protected override IEnumerator OnCardDraw(Team t)
    {
		if (t == team)
		{
			HandCard c = GameManager.Instance.GetHandCards()[0];
			if (c.orig.type == Type.Trick) c.ChangeCost(-1);
		}
        return base.OnCardDraw(t);
    }

    protected override IEnumerator OnCardDeath(Tuple<Card, Card> died)
	{
		if (died.Item1 == this && GameManager.Instance.team == team)
		{
            foreach (HandCard hc in GameManager.Instance.GetHandCards()) if (hc.orig.type == Type.Trick) hc.ChangeCost(1);
        }
		yield return base.OnCardDeath(died);
	}

}