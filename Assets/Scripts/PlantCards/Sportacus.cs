using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sportacus : Card
{

	protected override IEnumerator OnCardPlay(Card played)
	{
		if (played.type == Type.Trick && played.team == Team.B)
		{
            yield return Glow();
            yield return AttackFX(GameManager.Instance.GetTeamHero(GetOpponent(team)));
			yield return GameManager.Instance.GetTeamHero(GetOpponent(team)).ReceiveDamage(2, this, bullseye > 0);
		}
        yield return base.OnCardPlay(played);
    }

}
