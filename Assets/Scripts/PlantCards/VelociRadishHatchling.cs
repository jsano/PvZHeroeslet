using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelociRadishHatchling : Card
{

    protected override IEnumerator OnCardDraw(Team team)
    {
        if (team == this.team)
        {
            yield return Glow();
            ChangeStats(1, 0);
        }
        yield return base.OnCardDraw(team);
    }

}
