using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stompadon : Card
{

	protected override IEnumerator OnCardDraw(Team team)
	{
		if (team == this.team)
		{
			yield return new WaitForSeconds(1);
			if (GameManager.Instance.team == team) foreach (HandCard hc in GameManager.Instance.GetHandCards()) if (hc.orig.type == Type.Unit)
					{
						hc.ChangeAttack(1);
						hc.ChangeHP(1);
					}
        }
		yield return base.OnCardDraw(team);
	}

}
