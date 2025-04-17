using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skyshooter : Card
{

	protected override IEnumerator OnThisPlay()
	{
		if (col == 0)
		{
			yield return new WaitForSeconds(1);
			Heal(2, true);
			RaiseAttack(2);
		}
		yield return base.OnThisPlay();
	}

}
