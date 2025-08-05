using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sportacus : Card
{

	protected override IEnumerator OnCardPlay(Card played)
	{
		if (played.type == Type.Trick && played.team == Team.Zombie)
		{
			yield return AttackFX(GameManager.Instance.zombieHero);
			yield return GameManager.Instance.zombieHero.ReceiveDamage(2, this, bullseye > 0);
		}
        yield return base.OnCardPlay(played);
    }

}
