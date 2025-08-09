using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gargologist : Card
{

	protected override IEnumerator OnThisPlay()
	{
		if (GameManager.Instance.team == team) foreach (HandCard hc in GameManager.Instance.GetHandCards()) if (hc.orig.tribes.Contains(Tribe.Gargantuar)) hc.ChangeCost(-2);
		yield return base.OnThisPlay();
	}

    protected override IEnumerator OnCardDraw(Team t)
    {
		if (t == Team.Zombie && GameManager.Instance.team == Team.Zombie)
		{
			HandCard c = GameManager.Instance.GetHandCards()[0];
			if (c.orig.tribes.Contains(Tribe.Gargantuar)) c.ChangeCost(-2);
		}
        return base.OnCardDraw(t);
    }

    protected override IEnumerator OnCardDeath(Tuple<Card, Card> died)
	{
		if (died.Item1 == this && GameManager.Instance.team == team)
		{
            foreach (HandCard hc in GameManager.Instance.GetHandCards()) if (hc.orig.tribes.Contains(Tribe.Gargantuar)) hc.ChangeCost(2);
        }
		yield return base.OnCardDeath(died);
	}

}