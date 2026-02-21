using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RudeAwakening : Buff
{

    protected override IEnumerator OnCardPlay(Card played)
    {
        if (played.team != team)
        {
            if (played.type == Card.Type.Unit && played.HP > 1)
            {
                yield return played.ReceiveDamage(1, null);
            }
        }
        yield return base.OnCardPlay(played);
    }

}
