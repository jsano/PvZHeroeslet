using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlowSuffocation : Buff
{

    protected override void Start()
    {
        if (team == GameManager.Instance.team)
        {
            GameManager.Instance.turnTimerMax += 10;
            GameManager.Instance.mulliganTimerMax += 10;
            GameManager.Instance.blockTimerMax += 10;
            GameManager.Instance.buffTimerMax += 10;
        }
        StartCoroutine(Glow());
        base.Start();
    }

}
