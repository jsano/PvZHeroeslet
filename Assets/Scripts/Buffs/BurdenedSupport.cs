using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurdenedSupport : Buff
{

    protected override IEnumerator OnCardPlay(Card played)
    {
        if (played.team == team && played.type == Card.Type.Unit &&
            Tile.GetTeamTiles(team)[0, played.col].HasRevealedPlanted() && Tile.GetTeamTiles(team)[1, played.col].HasRevealedPlanted())
        {
                Tile.GetTeamTiles(team)[1, played.col].planted.ChangeStats(0, 3);
        }
        yield return base.OnCardPlay(played);
    }

}
