using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BrittleShield : Buff
{

    protected override void Start()
    {
        GameManager.Instance.GetTeamHero(team).segmentsToActivation = 6;
        base.Start();
    }

}
