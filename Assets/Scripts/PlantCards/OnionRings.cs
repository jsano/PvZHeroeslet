using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnionRings : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		if (GameManager.Instance.team == team)
			foreach (HandCard hc in GameManager.Instance.GetHandCards())
			{
				hc.ChangeAttack(4, true);
                hc.ChangeHP(4, true);
            }
		yield return base.OnThisPlay();
	}

}