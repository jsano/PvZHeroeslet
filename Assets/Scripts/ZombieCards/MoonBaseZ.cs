using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonBaseZ : Card
{

    protected override IEnumerator OnThisPlay()
    {
        for (int r = 0; r < Tile.ROWS; r++) if (Tile.GetTeamTiles(team)[r, col].HasRevealedPlanted()) Tile.GetTeamTiles(team)[r, col].planted.overshoot = Math.Max(Tile.GetTeamTiles(team)[r, col].planted.baseOvershoot, 3);
        yield return base.OnThisPlay();
    }

    protected override void OnCardPlayImmediate(Card played)
    {
        if (played.type == Type.Unit && played.team == team && played.col == col)
        {
            played.overshoot = Math.Max(Tile.GetTeamTiles(team)[0, col].planted.baseOvershoot, 3);
        }
    }

    protected override IEnumerator OnCardMoved(Card moved)
    {
        if (moved.oldCol == col && moved.col != col && moved.team == team)
        {
            moved.overshoot = Math.Max(Tile.GetTeamTiles(team)[0, col].planted.baseOvershoot, 0);
        }
        if (moved.oldCol != col && moved.col == col && moved.team == team)
        {
            moved.overshoot = Math.Max(Tile.GetTeamTiles(team)[0, col].planted.baseOvershoot, 3);
        }
        yield return base.OnCardMoved(moved);
    }

    protected override IEnumerator OnCardDeath(Tuple<Card, Card> died)
    {
        if (died.Item1 == this) for (int r = 0; r < Tile.ROWS; r++) if (Tile.GetTeamTiles(team)[r, col].HasRevealedPlanted()) Tile.GetTeamTiles(team)[r, col].planted.overshoot = Math.Max(Tile.GetTeamTiles(team)[r, col].planted.baseOvershoot, 0);
        yield return base.OnCardDeath(died);
    }

    void OnDestroy()
    {
        if (died) return;
        for (int r = 0; r < Tile.ROWS; r++) if (Tile.GetTeamTiles(team)[r, col].HasRevealedPlanted()) Tile.GetTeamTiles(team)[r, col].planted.overshoot = Math.Max(Tile.GetTeamTiles(team)[r, col].planted.baseOvershoot, 0);
    }

}
