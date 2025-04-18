using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PepperMD : Card
{

    protected override IEnumerator OnHeal(Card healed)
	{
		if (healed.team == Team.Plant)
		{
			yield return new WaitForSeconds(1);
			RaiseAttack(2);
			Heal(2, true);
		}
		yield return null;
	}

}
