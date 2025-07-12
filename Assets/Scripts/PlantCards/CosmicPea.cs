using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosmicPea : Card
{

    protected override IEnumerator OnThisPlay()
    {
        yield return new WaitForSeconds(1);
        int id = AllCards.RandomFromTribe((Tribe.Pea, Tribe.Pea));
        FinalStats fs = new(id, true);
        fs.abilities += "doubleStrike";
        yield return GameManager.Instance.GainHandCard(team, id, fs);
        yield return base.OnThisPlay();
    }

}
