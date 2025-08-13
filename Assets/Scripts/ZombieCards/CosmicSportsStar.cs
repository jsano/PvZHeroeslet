using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CosmicSportsStar : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return Glow();
        int id = AllCards.RandomFromTribe((Tribe.Sports, Tribe.Sports));
        FinalStats fs = new(id, true);
        fs.cost -= 2;
        yield return GameManager.Instance.GainHandCard(team, id, fs);
        yield return base.OnThisPlay();
	}

}
