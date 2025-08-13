using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overstuffed : Card
{

    protected override IEnumerator OnCardDeath(Tuple<Card, Card> died)
    {
        if (died.Item1.team == Team.Plant && died.Item2 == this && !died.Item2.died)
        {
            yield return Glow();
            StartCoroutine(Heal(2000));
            yield return GameManager.Instance.zombieHero.Heal(2);
        }
        yield return base.OnCardDeath(died);
    }

}
