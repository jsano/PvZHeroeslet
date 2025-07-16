using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GainOnFusion : Card
{

	public int atkAmount;
	public int HPAmount;

	protected override IEnumerator Fusion(Card parent)
	{
		parent.ChangeStats(atkAmount, HPAmount);
		yield return base.Fusion(parent);
    }

}
