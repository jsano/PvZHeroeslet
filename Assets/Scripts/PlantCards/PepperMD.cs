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
			RaiseAttack(2);
			Heal(2, true);
		}
		yield return null;
	}
	
	protected override IEnumerator OnHeroHeal(Hero healed)
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
