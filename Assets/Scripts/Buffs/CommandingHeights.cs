using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandingHeights : Buff
{

    protected override IEnumerator OnCardPlay(Card played)
    {
        if (played.team == team && played.type == Card.Type.Unit && played.col == Tile.HEIGHTS)
        {
            StartCoroutine(Glow());
            played.armor += 1;
        }
        yield return base.OnCardPlay(played);
    }

}
