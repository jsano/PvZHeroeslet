using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trickster : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return BonusAttack();
		yield return base.OnThisPlay();
	}

}
