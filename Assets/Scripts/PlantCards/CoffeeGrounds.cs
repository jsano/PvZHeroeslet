using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeGrounds : Card
{

    protected override IEnumerator OnThisPlay()
    {
        for (int i = 0; i < Tile.ROWS; i++) if (Tile.GetTeamTiles(team)[i, col].planted != null) Tile.GetTeamTiles(team)[i, col].planted.doubleStrike += 1;
        yield return base.OnThisPlay();
    }

    protected override void OnCardPlayImmediate(Card played)
    {
        if (played.type == Type.Unit && played.team == team && played.col == col)
        {
            played.doubleStrike += 1;
        }
    }

    protected override IEnumerator OnCardMoved(Card moved)
    {
        if (moved.oldCol == col && moved.col != col && moved.team == team)
        {
            moved.doubleStrike -= 1;
        }
        if (moved.oldCol != col && moved.col == col && moved.team == team)
        {
            moved.doubleStrike += 1;
        }
        yield return base.OnCardMoved(moved);
    }

    protected override IEnumerator OnCardDeath(Tuple<Card, Card> died)
    {
        if (died.Item1 == this) for (int i = 0; i < Tile.ROWS; i++) if (Tile.GetTeamTiles(team)[i, col].planted != null) Tile.GetTeamTiles(team)[i, col].planted.doubleStrike -= 1;
        yield return base.OnCardDeath(died);
    }

    void OnDestroy()
    {
        if (died) return;
        for (int i = 0; i < Tile.ROWS; i++) if (Tile.GetTeamTiles(team)[i, col].planted != null) Tile.GetTeamTiles(team)[i, col].planted.doubleStrike -= 1;
    }

}
