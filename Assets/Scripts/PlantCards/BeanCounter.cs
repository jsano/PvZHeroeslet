using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeanCounter : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return Glow();
		for (int i = 0; i < 2; i++) yield return GameManager.Instance.GainHandCard(team, AllCards.NameToID("Weenie Beanie"));
		yield return base.OnThisPlay();
	}

    protected override IEnumerator OnCardPlay(Card played)
    {
		if (played.tribes.Contains(Tribe.Bean))
		{
            yield return Glow();
            ChangeStats(1, 1);
		}
       yield return base.OnCardPlay(played);
    }

}
