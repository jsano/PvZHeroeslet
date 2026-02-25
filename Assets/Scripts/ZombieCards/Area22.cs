using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area22 : Card
{

    protected override IEnumerator OnThisPlay()
    {
        for (int r = 0; r < Tile.ROWS; r++) if (Tile.GetTeamTiles(team)[r, col].planted != null)
        {
            Tile.GetTeamTiles(team)[r, col].planted.ChangeStats(2, 2);
            Tile.GetTeamTiles(team)[r, col].planted.frenzy += 1;
        }
        yield return base.OnThisPlay();
    }

    protected override void OnCardPlayImmediate(Card played)
    {
        if (played.type == Type.Unit && played.team == team && played.col == col)
        {
            played.ChangeStats(2, 2);
            played.frenzy += 1;
        }
    }

    protected override IEnumerator OnCardMoved(Card moved)
    {
        if (moved.oldCol == col && moved.col != col && moved.team == team)
        {
            moved.ChangeStats(-2, -2);
            moved.frenzy -= 1;
        }
        if (moved.oldCol != col && moved.col == col && moved.team == team)
        {
            moved.ChangeStats(2, 2);
            moved.frenzy += 1;
        }
        yield return base.OnCardMoved(moved);
    }

    protected override IEnumerator OnCardDeath(Tuple<Card, Card> died)
    {
        if (died.Item1 == this)
        {
            for (int r = 0; r < Tile.ROWS; r++) if (Tile.GetTeamTiles(team)[r, col].planted != null)
            {
                Tile.GetTeamTiles(team)[r, col].planted.ChangeStats(-2, -2);
                Tile.GetTeamTiles(team)[r, col].planted.frenzy -= 1;
            }
        }
        yield return base.OnCardDeath(died);
    }

    void OnDestroy()
    {
        if (died) return;
        for (int r = 0; r < Tile.ROWS; r++) if (Tile.GetTeamTiles(team)[r, col].planted != null)
        {
            Tile.GetTeamTiles(team)[r, col].planted.ChangeStats(-2, -2);
            Tile.GetTeamTiles(team)[r, col].planted.frenzy -= 1;
        }
    }

}
