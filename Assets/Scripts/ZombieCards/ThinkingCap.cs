using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ThinkingCap : Card
{

	protected override IEnumerator OnThisPlay()
	{
        yield return new WaitForSeconds(1);
        for (int i = 0; i < 2; i++) yield return GameManager.Instance.GainHandCard(team, AllCards.RandomFromTribe((Tribe.Superpower, Tribe.Superpower), false, false, Team.Zombie));
        yield return base.OnThisPlay();
	}

}
