using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class TricksterHandCard : HandCard
{

    protected override IEnumerator OnCardPlay(Card played)
    {
        if (played.team == Card.Team.Zombie && played.type == Card.Type.Trick) ChangeCost(-1);
        return base.OnCardPlay(played);
    }

}
