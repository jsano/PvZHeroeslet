using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LieutenantCarrotron : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		GameManager.Instance.GainHandCard(team, AllCards.RandomFromTribe((Tribe.Root, Tribe.Root)));
		yield return base.OnThisPlay();
	}

}