using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CryoBrain : Card
{
	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		if (GameManager.Instance.team == team) GameManager.Instance.permanentBonus += 1;
		else GameManager.Instance.opponentPermanentBonus += 1;
		yield return base.OnThisPlay();
	}

}
