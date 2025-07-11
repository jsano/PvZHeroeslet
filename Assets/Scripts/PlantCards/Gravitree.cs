using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravitree : Card
{

    protected override IEnumerator OnCardPlay(Card played)
    {
        if (played.type == Type.Unit && played.team == Team.Zombie && played.col != col && Tile.zombieTiles[0, col].planted == null)
        {
            yield return new WaitForSeconds(1);
            played.Move(0, col);
        }
        yield return base.OnCardPlay(played);
    }

}
