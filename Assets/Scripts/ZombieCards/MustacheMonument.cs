using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MustacheMonument : Card
{

	protected override IEnumerator Fusion(Card parent)
	{
		yield return parent.BonusAttack();
		yield return base.Fusion(parent);
    }

}
