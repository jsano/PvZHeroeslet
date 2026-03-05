using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GuardBreak : Buff
{

    protected override void Start()
    {
        StartCoroutine(Glow());
        GameManager.Instance.GetTeamHero(Card.GetOpponent(team)).blockActivationLimit -= 1;
        base.Start();
    }

}
