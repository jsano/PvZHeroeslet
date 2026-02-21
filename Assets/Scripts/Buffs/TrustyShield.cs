using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrustyShield : Buff
{

    protected override void Start()
    {
        GameManager.Instance.GetTeamHero(team).blockActivationLimit += 2;
        base.Start();
    }

}
