using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeNut : Card
{

	protected override IEnumerator OnCardPlay(Card played)
	{
		if (played != this && played.type == Type.Unit && played.team == team)
		{
            yield return Glow();
            played.ChangeStats(-played.baseAtk + 3, 0);
		}
		yield return base.OnCardPlay(played);
	}

}
