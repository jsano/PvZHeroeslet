using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingOfTheGrill : Card
{

    protected override IEnumerator OnCardDeath(Tuple<Card, Card> died)
    {
        if (died.Item1.team == Team.Plant && died.Item2 != null && died.Item2.tribes.Contains(Tribe.Gargantuar))
        {
            yield return Glow();
            yield return GameManager.Instance.GainHandCard(team, AllCards.RandomFromTribe((Tribe.Gourmet, Tribe.Gourmet)));
        }
        yield return base.OnCardDeath(died);
    }

}
