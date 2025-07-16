using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tricorn : Card
{

	protected override IEnumerator OnThisPlay()
	{
		if (evolved)
		{
			yield return new WaitForSeconds(1);
			ChangeStats(2, 0);
		}
		yield return base.OnThisPlay();
	}

}