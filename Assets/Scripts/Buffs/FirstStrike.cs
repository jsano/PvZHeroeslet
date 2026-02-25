using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FirstStrike : Buff
{

    protected override IEnumerator OnTurnStart()
    {
        if (GameManager.Instance.WentFirst() == team) 
        {
            yield return GameManager.Instance.UpdateRemaining(1, team);
        }
        yield return base.OnTurnStart();
    }

}
