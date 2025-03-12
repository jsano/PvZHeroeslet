using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilBuddy : Card
{

	protected override IEnumerator OnThisPlay()
	{
		GameManager.Instance.plantHero.Heal(2);
		yield return base.OnThisPlay();
	}

}
