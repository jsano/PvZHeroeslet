using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class ValkyrieHandCard : HandCard
{

    protected override IEnumerator OnCardDeath(Card died)
    {
        if (died.team == Card.Team.Zombie) ChangeAttack(2);
        return base.OnCardPlay(died);
    }

}
