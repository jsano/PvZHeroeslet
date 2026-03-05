using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CuttingClose : Buff
{

    protected override void Start()
    {
        if (team == GameManager.Instance.team)
        {
            GameManager.Instance.turnTimerMax -= 5;
            GameManager.Instance.mulliganTimerMax -= 5;
            GameManager.Instance.blockTimerMax -= 5;
            //GameManager.Instance.buffTimerMax -= 5;
            GameManager.Instance.rerolls += 1;
        }
        StartCoroutine(Glow());
        base.Start();
    }

}
