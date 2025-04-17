using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonkChoy : Card
{
	/// Atk/HP change can be customized in inspector.
	/// Thus can be used for any card (ie. Tennis Champ) with this condition
	public int atkBuff = 0;
	public int HPBuff = 0;
	private bool firstTurnEnded;

	protected override IEnumerator OnThisPlay()
	{		
        yield return new WaitForSeconds(1);
		RaiseAttack(atkBuff);
		Heal(HPBuff, true);
		firstTurnEnded = false;
        yield return base.OnThisPlay();
	}

	protected override IEnumerator OnTurnEnd()
	{
		if (!firstTurnEnded) {
			RaiseAttack(-atkBuff);
			Heal(-HPBuff, true);
			firstTurnEnded = true;
		}
		yield return null;
    }

}
