using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnifyingGrass : Card
{

	protected override IEnumerator OnThisPlay()
	{
        yield return new WaitForSeconds(1);
        if (GameManager.Instance.team == team) ChangeStats(GameManager.Instance.remainingTop, 0);
        else ChangeStats(GameManager.Instance.opponentRemainingTop, 0);
		yield return base.OnThisPlay();
	}

}
