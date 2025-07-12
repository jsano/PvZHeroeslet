using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosmicBean : Card
{

    protected override IEnumerator OnThisPlay()
    {
        yield return new WaitForSeconds(1);
        int id = AllCards.RandomFromTribe((Tribe.Bean, Tribe.Bean));
        FinalStats fs = new(id, true);
        fs.abilities += "teamUp";
        yield return GameManager.Instance.GainHandCard(team, id, fs);
        yield return base.OnThisPlay();
    }

}
