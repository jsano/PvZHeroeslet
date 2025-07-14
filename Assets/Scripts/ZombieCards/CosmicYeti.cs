using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CosmicYeti : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
        int id = AllCards.RandomFromTribe((Tribe.Pet, Tribe.Pet));
        FinalStats fs = new(id, true);
        fs.atk += 1;
        fs.hp += 1;
        yield return GameManager.Instance.GainHandCard(team, id, fs);
        yield return base.OnThisPlay();
	}

    protected override IEnumerator OnTurnEnd()
    {
		Bounce();
        return base.OnTurnEnd();
    }

}
