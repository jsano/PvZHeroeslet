using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoodOfVigor : Buff
{

    protected override IEnumerator OnCardDeath(Tuple<Card, Card> died)
    {
        if (died.Item1.team == team && died.Item1._class == Card.Class.Elation)
        {
            StartCoroutine(Glow());
            yield return GameManager.Instance.UpdateRemaining(died.Item1.playedCost, team);
        }
        yield return base.OnCardDeath(died);
    }

}
