using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilBuddy : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return GameManager.Instance.plantHero.Heal(2);
		yield return base.OnThisPlay();
	}

}
