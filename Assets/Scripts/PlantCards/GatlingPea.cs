using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatlingPea : Card
{

	protected override IEnumerator OnThisPlay()
	{
		if (evolved)
		{
            yield return Glow();
            yield return BonusAttack();
		}
		yield return base.OnThisPlay();
	}

}
