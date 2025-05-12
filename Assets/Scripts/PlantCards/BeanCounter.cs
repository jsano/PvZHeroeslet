using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeanCounter : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		for (int i = 0; i < 2; i++) yield return GameManager.Instance.GainHandCard(team, AllCards.NameToID("Weenie Beanie"));
		yield return base.OnThisPlay();
	}

    protected override IEnumerator OnCardPlay(Card played)
    {
		if (played.tribes.Contains(Tribe.Bean)) ChangeStats(1, 1);
        return base.OnCardPlay(played);
    }

}
