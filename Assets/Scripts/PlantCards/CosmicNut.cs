using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosmicNut : Card
{

    protected override IEnumerator OnThisPlay()
    {
        yield return new WaitForSeconds(1);
        int id = AllCards.RandomFromTribe((Tribe.Nut, Tribe.Nut));
        FinalStats fs = new(id, true);
        fs.atk = 3;
        yield return GameManager.Instance.GainHandCard(team, id, fs);
        yield return base.OnThisPlay();
    }

}
