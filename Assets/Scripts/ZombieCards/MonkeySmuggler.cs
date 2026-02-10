using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkeySmuggler : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return Glow();
		int amount = GameManager.Instance.GetTeamHero(GetOpponent(team)).StealBlock(1);
		GameManager.Instance.GetTeamHero(team).StealBlock(-amount);
		yield return base.OnThisPlay();
	}

}
