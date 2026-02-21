using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShortFuse : Buff
{

    private int state = 0; // 0 = +3, 1 = -2, 2 = done

    protected override IEnumerator OnTurnStart()
    {
        if (state == 0) yield return GameManager.Instance.UpdateRemaining(3, team);
        else if (state == 1) yield return GameManager.Instance.UpdateRemaining(-2, team);
        state += 1;
        yield return base.OnTurnStart();
    }

}
