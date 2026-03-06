using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedullaNebula : Card
{

    protected override IEnumerator OnCardPlay(Card played)
    {
        if (played.type == Type.Unit && played.team == team && played.col == col)
        {
            yield return Glow();
            yield return GameManager.Instance.UpdateRemaining(2, team);
        }
        yield return base.OnCardPlay(played);
    }

}
