using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astroshroom : Card
{

	protected override IEnumerator OnCardPlay(Card played)
	{
		if (played != this && played.type == Type.Unit && played.team == team)
		{
            yield return Glow();
            yield return AttackFX(GameManager.Instance.GetTeamHero(GetOpponent(team)));
			yield return GameManager.Instance.GetTeamHero(GetOpponent(team)).ReceiveDamage(1, this, bullseye > 0);
		}
		yield return base.OnCardPlay(played);
	}

}
