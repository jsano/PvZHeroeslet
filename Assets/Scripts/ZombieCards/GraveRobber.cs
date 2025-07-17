using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveRobber : Card
{

	protected override IEnumerator OnCardPlay(Card played)
	{
		if (played != this && played.baseGravestone)
		{
			yield return new WaitForSeconds(1);
			ChangeStats(1, 0);
		}
		yield return base.OnCardPlay(played);
	}

}
