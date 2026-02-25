using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HarmonicStrike : Buff
{

    protected override IEnumerator OnTurnStart()
    {
        HashSet<Card.Class> done = new();
        foreach (Transform t in GameManager.Instance.playerBuffs)
        {
            Buff b = t.GetComponent<Buff>();
            if (b.buffClass != Card.Class.Awe) done.Add(b.buffClass);
        }
        if (done.Count > 0) 
        {
            yield return GameManager.Instance.GetTeamHero(Card.GetOpponent(team)).ReceiveDamage(done.Count, null);
        }
        yield return base.OnTurnStart();
    }

}
