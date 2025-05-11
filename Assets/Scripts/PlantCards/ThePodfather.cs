using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThePodfather : Card
{

	protected override IEnumerator OnCardPlay(Card played)
	{
		if (played != this && played.tribes.Contains(Tribe.Pea))
		{
			played.RaiseAttack(2);
			played.Heal(2, true);
		}
		yield return base.OnCardPlay(played);
	}

}
