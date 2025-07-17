using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkeySmuggler : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		int amount = GameManager.Instance.plantHero.StealBlock(1);
		GameManager.Instance.zombieHero.StealBlock(-amount);
		yield return base.OnThisPlay();
	}

}
