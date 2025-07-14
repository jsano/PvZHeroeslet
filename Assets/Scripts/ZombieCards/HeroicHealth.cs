using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroicHealth : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		yield return GameManager.Instance.zombieHero.Heal(6);
		yield return base.OnThisPlay();
	}

}
