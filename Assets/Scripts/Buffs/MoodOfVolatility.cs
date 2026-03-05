using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class MoodOfVolatility : Buff
{

    private List<Card> affectedCards = new ();

    protected override IEnumerator OnCardPlay(Card played)
    {
        if (played.team == team)
        {
            if (played._class == Card.Class.Wrath && played.type == Card.Type.Unit)
            {
                StartCoroutine(Glow());
                played.ChangeStats(4, 0);
                affectedCards.Add(played);
            }
        }
        yield return base.OnCardPlay(played);
    }

    protected override IEnumerator OnTurnStart()
    {
        if (affectedCards.Count > 0) StartCoroutine(Glow());
        foreach (Card card in affectedCards)
        {
            if (card == null) continue;
            card.Destroy();
        }
        return base.OnTurnStart();
    }

}
