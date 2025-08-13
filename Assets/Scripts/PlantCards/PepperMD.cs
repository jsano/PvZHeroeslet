using System;
using System.Collections;
using UnityEngine;

public class PepperMD : Card
{

    protected override IEnumerator OnCardHeal(Tuple<Card, int> healed)
	{
		if (healed.Item1.team == Team.Plant)
		{
            yield return Glow();
            ChangeStats(2, 2);
		}
		yield return null;
	}
	
	protected override IEnumerator OnHeroHeal(Tuple<Hero, int> healed)
    {
        if (healed.Item1.team == Team.Plant)
		{
			yield return Glow();
			ChangeStats(2, 2);
		}
		yield return null;
    }

}
