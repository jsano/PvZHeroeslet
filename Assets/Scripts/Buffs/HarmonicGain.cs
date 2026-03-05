using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HarmonicGain : Buff
{

    private HashSet<Card.Class> done = new();

    protected override IEnumerator OnCardPlay(Card played)
    {
        if (played.team == team) 
        {
            if (!done.Contains(played._class))
            {
                StartCoroutine(Glow());
                yield return GameManager.Instance.UpdateRemaining(1, team);
            }
            done.Add(played._class);
        }
        yield return base.OnCardPlay(played);
    }

}
