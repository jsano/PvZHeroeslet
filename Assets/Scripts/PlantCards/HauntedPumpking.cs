using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HauntedPumpking : Card
{

	protected override IEnumerator OnThisPlay()
	{
        yield return new WaitForSeconds(1);
        yield return GameManager.Instance.GainHandCard(Team.Zombie, AllCards.RandomFromTribe((Tribe.Monster, Tribe.Monster)));
        yield return base.OnThisPlay();
	}

}
