using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeGrounds : Card
{

    protected override IEnumerator OnThisPlay()
    {
        for (int i = 0; i < 2; i++) if (Tile.plantTiles[i, col].planted != null) Tile.plantTiles[i, col].planted.doubleStrike += 1;
        yield return base.OnThisPlay();
    }

    protected override IEnumerator OnCardPlay(Card played)
    {
        if (played.type == Type.Unit && played.team == Team.Plant && played.col == col)
        {
            played.doubleStrike += 1;
        }
        yield return base.OnCardPlay(played);
    }

    protected override IEnumerator OnCardMoved(Card moved)
    {
        if (moved.oldCol == col && moved.team == Team.Plant)
        {
            moved.doubleStrike -= 1;
        }
        if (moved.col == col && moved.team == Team.Plant)
        {
            moved.doubleStrike += 1;
        }
        yield return base.OnCardMoved(moved);
    }

    protected override IEnumerator OnCardDeath(Card died)
    {
        if (died == this) for (int i = 0; i < 2; i++) if (Tile.plantTiles[i, col].planted != null) Tile.plantTiles[i, col].planted.doubleStrike -= 1;
        yield return base.OnCardDeath(died);
    }

}
