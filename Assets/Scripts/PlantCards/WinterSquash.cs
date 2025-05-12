using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinterSquash : Card
{

	protected override IEnumerator OnCardFreeze(Card frozen)
	{
		if (frozen.team == Team.Zombie && !frozen.died)
		{
			yield return new WaitForSeconds(1);
			frozen.Destroy();
		}
		yield return null;
	}

}

