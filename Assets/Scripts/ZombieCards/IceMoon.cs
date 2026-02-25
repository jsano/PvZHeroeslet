using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceMoon : Card
{

    protected override IEnumerator OnThisPlay()
    {
        for (int i = 0; i < 2; i++) if (Tile.GetTeamTiles(GetOpponent(team))[i, col].planted != null && Tile.GetTeamTiles(GetOpponent(team))[i, col].planted.untrickable == 0)
        {
            Tile.GetTeamTiles(GetOpponent(team))[i, col].planted.Freeze();
        }
        for (int r = 0; r < Tile.ROWS; r++) if (Tile.GetTeamTiles(team)[r, col].HasRevealedPlanted()) Tile.GetTeamTiles(team)[r, col].planted.strikethrough += 1;
        yield return base.OnThisPlay();
    }

    protected override void OnCardPlayImmediate(Card played)
    {
        if (played.type == Type.Unit && played.team == team && played.col == col)
        {
            played.strikethrough += 1;
        }
    }

    protected override IEnumerator OnCardMoved(Card moved)
    {
        if (moved.oldCol == col && moved.col != col && moved.team == team)
        {
            moved.strikethrough -= 1;
        }
        if (moved.oldCol != col && moved.col == col && moved.team == team)
        {
            moved.strikethrough += 1;
        }
        yield return base.OnCardMoved(moved);
    }

    protected override IEnumerator OnCardDeath(Tuple<Card, Card> died)
    {
        if (died.Item1 == this) for (int r = 0; r < Tile.ROWS; r++) if (Tile.GetTeamTiles(team)[r, col].HasRevealedPlanted()) Tile.GetTeamTiles(team)[r, col].planted.strikethrough -= 1;
        yield return base.OnCardDeath(died);
    }

    void OnDestroy()
    {
        if (died) return;
        for (int r = 0; r < Tile.ROWS; r++) if (Tile.GetTeamTiles(team)[r, col].HasRevealedPlanted()) Tile.GetTeamTiles(team)[r, col].planted.strikethrough -= 1;
    }

}
