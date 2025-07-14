using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CosmicDancer : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
        int id = AllCards.RandomFromTribe((Tribe.Dancing, Tribe.Dancing));
        FinalStats fs = new(id, true);
        fs.abilities += "overshoot2";
        yield return GameManager.Instance.GainHandCard(team, id, fs);
        yield return base.OnThisPlay();
	}

}
