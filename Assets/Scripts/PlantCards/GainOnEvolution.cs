using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GainOnEvolution : Card
{

	public int atkAmount;
	public int HPAmount;

	protected override IEnumerator OnThisPlay()
	{
		if (evolved)
		{
			yield return new WaitForSeconds(1);
			ChangeStats(atkAmount, HPAmount);
		}
		yield return base.OnThisPlay();
	}

}