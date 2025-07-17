using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoDanceFloor : Card
{

	protected override IEnumerator Fusion(Card parent)
	{
		parent.overshoot = Mathf.Max(2, parent.overshoot);
		yield return base.Fusion(parent);
    }

}
