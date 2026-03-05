using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeldBreath : Buff
{

    private bool active;

    protected override void OnCardPlayImmediate(Card played)
    {
        if (played.team == team && played.type == Card.Type.Unit)
        {
            active = false;
        }
        base.OnCardPlayImmediate(played);
    }

    protected override IEnumerator OnTurnStart()
    {
        if (active)
        {
            StartCoroutine(Glow());
            if (team == GameManager.Instance.team) foreach (HandCard hc in GameManager.Instance.GetHandCards()) if (hc.orig.type == Card.Type.Unit) hc.ChangeCost(-2);
        }
        active = true;
        return base.OnTurnStart();
    }

}
