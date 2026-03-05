using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnduringResolve : Buff
{

    protected override void Start()
    {
        StartCoroutine(Glow());
        Hero h = GameManager.Instance.GetTeamHero(team);
        h.blockActivationLimit -= 10;
        h.ChangeStats(0, 15, true);
        base.Start();
    }

}
