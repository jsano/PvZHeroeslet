using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LieutenantCarrotron : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		yield return GameManager.Instance.GainHandCard(team, AllCards.RandomFromTribe((Tribe.Otherwordly, Tribe.Otherwordly)));
		yield return base.OnThisPlay();
	}

}