using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Yeti : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		yield return GameManager.Instance.GainHandCard(team, AllCards.NameToID("Yeti Lunchbox"));
		yield return base.OnThisPlay();
	}

    protected override IEnumerator OnTurnEnd()
    {
		Bounce();
        return base.OnTurnEnd();
    }

}
