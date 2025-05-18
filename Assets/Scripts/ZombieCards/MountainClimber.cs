using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountainClimber : Card
{

	protected override IEnumerator OnThisPlay()
	{
		if (col == 0)
		{
			yield return new WaitForSeconds(1);
			ChangeStats(2, 2);
		}
		yield return base.OnThisPlay();
	}

}
