using System;
using System.Collections;
using UnityEngine;

public class PepperMD : Card
{

    protected override IEnumerator OnCardHeal(Card healed)
	{
		if (healed.team == Team.Plant)
		{
			yield return new WaitForSeconds(1);
			ChangeStats(2, 2);
		}
		yield return null;
	}
	
	protected override IEnumerator OnHeroHeal(Hero healed)
    {
        if (healed.team == Team.Plant)
		{
			yield return new WaitForSeconds(1);
			ChangeStats(2, 2);
		}
		yield return null;
    }

}
