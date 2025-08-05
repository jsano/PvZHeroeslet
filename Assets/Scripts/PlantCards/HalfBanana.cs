using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfBanana : Card
{

    protected override IEnumerator OnCardDeath(Tuple<Card, Card> died)
	{
		if (died.Item1 == this && GameManager.Instance.team == team)
		{
			foreach (HandCard hc in GameManager.Instance.GetHandCards()) if (hc.orig.type == Type.Unit && hc.orig.tribes.Contains(Tribe.Fruit)) {
					hc.ChangeAttack(1);
					hc.ChangeHP(1);
			}
        }
		yield return base.OnCardDeath(died);
	}

}