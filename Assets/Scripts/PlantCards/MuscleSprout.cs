using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuscleSprout : Card
{

	protected override IEnumerator OnCardPlay(Card played)
	{
		if (played != this && played.type == Type.Unit && played.team == team)
		{
			yield return Glow();
			ChangeStats(1, 1);
		}
		yield return base.OnCardPlay(played);
	}

}
