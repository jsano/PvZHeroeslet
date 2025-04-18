using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorningGlory : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
        if ()
        {
            c.RaiseAttack(1);
            c.Heal(1, true);
        }
		yield return base.OnThisPlay();
	}

}
