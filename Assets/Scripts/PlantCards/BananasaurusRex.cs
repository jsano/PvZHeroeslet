using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananasaurusRex : Card
{

	protected override IEnumerator OnCardDraw(Team team)
	{
		if (team == this.team)
		{
			yield return new WaitForSeconds(1);
			ChangeStats(1, 1);
		}
		yield return base.OnCardDraw(team);
	}

}
