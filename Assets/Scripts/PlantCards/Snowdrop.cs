using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowdrop : Card
{

	protected override IEnumerator OnCardFreeze(Card frozen)
	{
		if (frozen.team == Team.Zombie)
		{
			yield return Glow();
			ChangeStats(2, 2);
		}
		yield return base.OnCardFreeze(frozen);
	}

}

