using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnifyingGrass : Card
{

	protected override IEnumerator OnThisPlay()
	{
        yield return new WaitForSeconds(1);
        if (GameManager.Instance.team == team) RaiseAttack(GameManager.Instance.remainingTop);
        else RaiseAttack(GameManager.Instance.opponentRemainingTop);
		yield return base.OnThisPlay();
	}

}
