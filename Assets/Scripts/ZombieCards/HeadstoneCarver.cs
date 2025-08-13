using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadstoneCarver : Card
{

	protected override IEnumerator OnCardPlay(Card played)
	{
		if (played != this && played.baseGravestone)
		{
			yield return Glow();
			played.ChangeStats(1, 1);
		}
		yield return base.OnCardPlay(played);
	}

}
