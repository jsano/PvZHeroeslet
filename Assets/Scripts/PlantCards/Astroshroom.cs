using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astroshroom : Card
{

	protected override IEnumerator OnCardPlay(Card played)
	{
		if (played != this && played.type == Type.Unit && played.team == Team.Plant)
		{
			yield return AttackFX(GameManager.Instance.zombieHero);
			yield return GameManager.Instance.zombieHero.ReceiveDamage(1, this, bullseye > 0);
		}
		yield return base.OnCardPlay(played);
	}

}
