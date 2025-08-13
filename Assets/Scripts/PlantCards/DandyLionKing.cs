using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DandyLionKing : Card
{

	protected override IEnumerator OnThisPlay()
	{
        yield return Glow();
        yield return AttackFX(GameManager.Instance.zombieHero);
		yield return GameManager.Instance.zombieHero.ReceiveDamage((int)Mathf.Floor(GameManager.Instance.zombieHero.HP / 2), this);

		yield return base.OnThisPlay();
	}

}
