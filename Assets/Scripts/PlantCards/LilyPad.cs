using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilyPad : Card
{

	protected override IEnumerator Fusion(Card parent)
	{
        yield return new WaitForSeconds(1);
        yield return GameManager.Instance.GainHandCard(team, AllCards.RandomFromTribe((Tribe.Leafy, Tribe.Leafy)));
        yield return base.Fusion(parent);
    }

}
