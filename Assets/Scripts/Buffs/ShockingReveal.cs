using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShockingReveal : Buff
{

    protected override void OnCardPlayImmediate(Card played)
    {
        if (played.baseGravestone)
        {
            played.ChangeStats(1, 0);
        }
        base.OnCardPlayImmediate(played);
    }

}
