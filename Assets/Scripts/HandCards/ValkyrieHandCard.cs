using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class ValkyrieHandCard : HandCard
{

    protected override IEnumerator OnCardDeath(Tuple<Card, Card> died)
    {
        if (died.Item1.team == Card.Team.Zombie && died.Item1.type == Card.Type.Unit) ChangeAttack(2);
        return base.OnCardDeath(died);
    }

}
