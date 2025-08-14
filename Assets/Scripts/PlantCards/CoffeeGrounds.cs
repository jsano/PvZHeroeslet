using System;
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

    protected override void OnCardPlayImmediate(Card played)
    {
        if (played.type == Type.Unit && played.team == Team.Plant && played.col == col)
        {
            played.doubleStrike += 1;
        }
    }

    protected override IEnumerator OnCardMoved(Card moved)
    {
        if (moved.oldCol == col && moved.col != col && moved.team == Team.Plant)
        {
            moved.doubleStrike -= 1;
        }
        if (moved.oldCol != col && moved.col == col && moved.team == Team.Plant)
        {
            moved.doubleStrike += 1;
        }
        yield return base.OnCardMoved(moved);
    }

    protected override IEnumerator OnCardDeath(Tuple<Card, Card> died)
    {
        if (died.Item1 == this) for (int i = 0; i < 2; i++) if (Tile.plantTiles[i, col].planted != null) Tile.plantTiles[i, col].planted.doubleStrike -= 1;
        yield return base.OnCardDeath(died);
    }

}
