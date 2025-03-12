using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paparazzi : Card
{

	protected override IEnumerator OnCardPlay(Card played)
	{
		if (played.type == Type.Trick && played.team == Team.Zombie)
		{
			yield return new WaitForSeconds(1);
			RaiseAttack(1);
			Heal(1, true);
		}
		yield return null;
	}

}
