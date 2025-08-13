using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosmicFlower : Card
{

    protected override IEnumerator OnThisPlay()
    {
        yield return Glow();
        int id = AllCards.RandomFromTribe((Tribe.Flower, Tribe.Flower));
        FinalStats fs = new(id, true);
        fs.abilities += "strikethrough";
        yield return GameManager.Instance.GainHandCard(team, id, fs);
        yield return base.OnThisPlay();
    }

}
