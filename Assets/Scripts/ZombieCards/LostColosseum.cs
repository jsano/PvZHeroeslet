using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostColosseum : Card
{

	protected override IEnumerator Fusion(Card parent)
	{
		parent.frenzy += 1;
		parent.ChangeStats(2, 3);
		yield return base.Fusion(parent);
    }

}
