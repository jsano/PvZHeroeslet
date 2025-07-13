using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cosmoss : Card
{

	protected override IEnumerator OnCardPlay(Card played)
	{
		if (played != this && played.type == Type.Terrain)
		{
			yield return new WaitForSeconds(1);
			ChangeStats(2, 2);
		}
		yield return base.OnCardPlay(played);
	}

}
