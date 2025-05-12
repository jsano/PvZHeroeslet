using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WannabeHero : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		GameManager.Instance.zombieHero.Heal(3);
		ChangeStats(0, GameManager.Instance.zombieHero.HP);
		yield return base.OnThisPlay();
	}

}
