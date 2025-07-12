using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodFighter : Card
{

	protected override IEnumerator OnCardPlay(Card played)
	{
		if (played != this && played.type == Type.Unit && played.team == Team.Plant && Mathf.Abs(played.col - col) <= 1)
		{
			yield return BonusAttack();
		}
		yield return base.OnCardPlay(played);
	}

}
