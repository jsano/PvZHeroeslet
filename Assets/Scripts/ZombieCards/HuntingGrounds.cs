using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntingGrounds : Card
{

    protected override IEnumerator OnThisPlay()
    {
        for (int r = 0; r < Tile.ROWS; r++) if (Tile.GetTeamTiles(team)[r, col].HasRevealedPlanted())
        {
            Tile.GetTeamTiles(team)[r, col].planted.hunt += 1;
        }
        yield return base.OnThisPlay();
    }

    protected override IEnumerator OnCardPlay(Card played)
    {
        if (played.type == Type.Unit && played.team == team && played.col == col)
        {
            played.hunt += 1;
            played.ChangeStats(1, 1);
        }
        yield return base.OnCardPlay(played);
    }

    protected override IEnumerator OnCardMoved(Card moved)
    {
        if (moved.oldCol == col && moved.col != col && moved.team == team)
        {
            moved.hunt -= 1;
            moved.ChangeStats(1, 1);
        }
        if (moved.oldCol != col && moved.col == col && moved.team == team)
        {
            moved.hunt += 1;
            moved.ChangeStats(1, 1);
        }
        yield return base.OnCardMoved(moved);
    }

    protected override IEnumerator OnCardDeath(Tuple<Card, Card> died)
    {
        if (died.Item1 == this) for (int r = 0; r < Tile.ROWS; r++) if (Tile.GetTeamTiles(team)[r, col].HasRevealedPlanted())
            {
                Tile.GetTeamTiles(team)[r, col].planted.hunt -= 1;
            }
        yield return base.OnCardDeath(died);
    }

}
