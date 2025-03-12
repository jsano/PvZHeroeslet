using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuscleSprout : Card
{

	protected override IEnumerator OnCardPlay(Card played)
	{
		if (played != this && played.team == Team.Plant)
		{
			yield return new WaitForSeconds(1);
			RaiseAttack(1);
			Heal(1, true);
		}
		yield return null;
	}

}
