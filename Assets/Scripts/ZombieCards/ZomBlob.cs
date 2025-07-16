using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZomBlob : Card
{

	protected override IEnumerator OnThisPlay()
	{
		if (evolved)
		{
			yield return new WaitForSeconds(1);
			ChangeStats(GameManager.Instance.team == Team.Zombie ? GameManager.Instance.remainingTop : GameManager.Instance.opponentRemainingTop, 0);
		}
		yield return base.OnThisPlay();
	}

}