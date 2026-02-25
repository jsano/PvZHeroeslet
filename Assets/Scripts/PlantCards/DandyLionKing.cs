using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DandyLionKing : Card
{

	protected override IEnumerator OnThisPlay()
	{
        yield return Glow();
        yield return AttackFX(GameManager.Instance.GetTeamHero(GetOpponent(team)));
		yield return GameManager.Instance.GetTeamHero(GetOpponent(team)).ReceiveDamage((int)Mathf.Floor(GameManager.Instance.GetTeamHero(GetOpponent(team)).HP / 2), this);

		yield return base.OnThisPlay();
	}

}
