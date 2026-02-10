using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravitree : Card
{

    protected override IEnumerator OnCardPlay(Card played)
    {
        if (played.type == Type.Unit && played.team == GetOpponent(team) && played.col != col && Tile.CanPlantInCol(col, Tile.GetTeamTiles(GetOpponent(team)), played.teamUp, played.amphibious))
        {
            yield return Glow();
            played.Move(0, col);
        }
        yield return base.OnCardPlay(played);
    }

}
