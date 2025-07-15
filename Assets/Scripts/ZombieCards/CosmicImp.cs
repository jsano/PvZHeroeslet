using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CosmicImp : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
        int id = AllCards.RandomFromTribe((Tribe.Imp, Tribe.Imp));
        FinalStats fs = new(id, true);
        fs.abilities += "deadly";
        yield return GameManager.Instance.GainHandCard(team, id, fs);
        yield return base.OnThisPlay();
	}

}
