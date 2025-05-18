using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmWrestler : Card
{

	protected override IEnumerator OnCardPlay(Card played)
	{
		if (played.col == col && played.team == Team.Plant)
		{
			yield return new WaitForSeconds(1);
			ChangeStats(1, 1);
		}
		yield return base.OnCardPlay(played);
	}

}
