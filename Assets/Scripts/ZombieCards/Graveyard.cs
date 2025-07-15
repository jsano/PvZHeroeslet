using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graveyard : Card
{

    protected override IEnumerator OnCardPlay(Card played)
    {
        if (played.type == Type.Unit && played.team == Team.Zombie && played.col == col && played.baseGravestone)
        {
            yield return new WaitForSeconds(1);
            played.ChangeStats(1, 0);
        }
        yield return base.OnCardPlay(played);
    }

}
