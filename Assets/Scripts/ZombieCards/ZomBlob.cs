using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZomBlob : Card
{

	protected override IEnumerator OnThisPlay()
	{
		if (evolved)
		{
			yield return Glow();
			ChangeStats(GameManager.Instance.team == team ? (int)GameManager.Instance.remainingTop : (int)GameManager.Instance.opponentRemainingTop, 0);
		}
		yield return base.OnThisPlay();
	}

}