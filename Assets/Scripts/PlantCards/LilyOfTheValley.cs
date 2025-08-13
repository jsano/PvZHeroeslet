using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilyOfTheValley : Card
{

    protected override IEnumerator OnCardPlay(Card played)
    {
        if (played != this && played.type == Type.Unit && played.team == Team.Plant && played.col == 0)
        {
            yield return Glow();
            played.ChangeStats(2, 2);
        }
        yield return base.OnCardPlay(played);
    }

}
