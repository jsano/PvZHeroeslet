using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosmicMushroom : Card
{

    protected override IEnumerator OnThisPlay()
    {
        yield return new WaitForSeconds(1);
        int id = AllCards.RandomFromTribe((Tribe.Mushroom, Tribe.Mushroom));
        FinalStats fs = new(id, true);
        fs.atk += 3;
        yield return GameManager.Instance.GainHandCard(team, id, fs);
        yield return base.OnThisPlay();
    }

}
