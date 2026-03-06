using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graveyard : Card
{

    protected override IEnumerator OnCardPlay(Card played)
    {
        if (played.type == Type.Unit && played.team == team && played.col == col && played.baseGravestone)
        {
            yield return Glow();
            played.ChangeStats(1, 0);
        }
        yield return base.OnCardPlay(played);
    }

}
