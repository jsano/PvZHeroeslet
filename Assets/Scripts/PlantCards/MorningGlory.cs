using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorningGlory : Card
{

	protected override IEnumerator OnThisPlay()
	{
        bool s = false;
        if (GameManager.Instance.team == team) 
        {
            if (GameManager.Instance.remainingTop >= 6) s = true;
        }
        else if (GameManager.Instance.opponentRemainingTop >= 6) s = true;
        if (s)
        {
            yield return new WaitForSeconds(1);
            RaiseAttack(1);
            Heal(1, true);
        }
		yield return base.OnThisPlay();
	}

}
