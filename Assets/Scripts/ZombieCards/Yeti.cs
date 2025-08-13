using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Yeti : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return Glow();
		yield return GameManager.Instance.GainHandCard(team, AllCards.NameToID("Yeti Lunchbox"));
		yield return base.OnThisPlay();
	}

    protected override IEnumerator OnTurnEnd()
    {
        yield return Glow();
        Bounce();
        yield return base.OnTurnEnd();
    }

}
