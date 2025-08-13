using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CosmicScientist : Card
{

	protected override IEnumerator OnThisPlay()
	{
        yield return Glow();
        int id = AllCards.RandomFromTribe((Tribe.Science, Tribe.Science));
        FinalStats fs = new(id, true);
        fs.abilities += "bullseye";
        yield return GameManager.Instance.GainHandCard(team, id, fs);
        yield return base.OnThisPlay();
	}

}
