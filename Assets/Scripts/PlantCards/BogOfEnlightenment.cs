using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BogOfEnlightenment : Card
{

    protected override IEnumerator OnThisPlay()
    {
        for (int i = 0; i < Tile.ROWS; i++) if (Tile.GetTeamTiles(team)[i, col].planted != null && Tile.GetTeamTiles(team)[i, col].planted.amphibious) Tile.GetTeamTiles(team)[i, col].planted.ChangeStats(2, 0);
        if (Tile.GetTeamTiles(GetOpponent(team))[0, col].HasRevealedPlanted() && !Tile.GetTeamTiles(GetOpponent(team))[0, col].planted.amphibious) Tile.GetTeamTiles(GetOpponent(team))[0, col].planted.ChangeStats(-2, 0);
        yield return base.OnThisPlay();
    }

    protected override void OnCardPlayImmediate(Card played)
    {
        if (played.type == Type.Unit && played.team == Team.A && played.col == col && played.amphibious) played.ChangeStats(2, 0);
        if (played.type == Type.Unit && played.team == Team.B && played.col == col && !played.amphibious) played.ChangeStats(-2, 0);
    }

    protected override IEnumerator OnCardMoved(Card moved)
    {
        if (moved.oldCol == col && moved.col != col && moved.team == Team.A && moved.amphibious) moved.ChangeStats(-2, 0);
        if (moved.oldCol == col && moved.col != col && moved.team == Team.B && !moved.amphibious) moved.ChangeStats(2, 0);
        
        if (moved.oldCol != col && moved.col == col && moved.team == Team.A && moved.amphibious) moved.ChangeStats(2, 0);
        if (moved.oldCol != col && moved.col == col && moved.team == Team.B && !moved.amphibious) moved.ChangeStats(-2, 0);
        yield return base.OnCardMoved(moved);
    }

    protected override IEnumerator OnCardDeath(Tuple<Card, Card> died)
    {
        if (died.Item1 == this)
        {
            for (int i = 0; i < Tile.ROWS; i++) if (Tile.GetTeamTiles(team)[i, col].planted != null && Tile.GetTeamTiles(team)[i, col].planted.amphibious) Tile.GetTeamTiles(team)[i, col].planted.ChangeStats(-2, 0);
            if (Tile.GetTeamTiles(GetOpponent(team))[0, col].HasRevealedPlanted() && !Tile.GetTeamTiles(GetOpponent(team))[0, col].planted.amphibious) Tile.GetTeamTiles(GetOpponent(team))[0, col].planted.ChangeStats(2, 0);
        }
        yield return base.OnCardDeath(died);
    }

    void OnDestroy()
    {
        if (died) return;
        for (int i = 0; i < Tile.ROWS; i++) if (Tile.GetTeamTiles(team)[i, col].planted != null && Tile.GetTeamTiles(team)[i, col].planted.amphibious) Tile.GetTeamTiles(team)[i, col].planted.ChangeStats(-2, 0);
        if (Tile.GetTeamTiles(GetOpponent(team))[0, col].HasRevealedPlanted() && !Tile.GetTeamTiles(GetOpponent(team))[0, col].planted.amphibious) Tile.GetTeamTiles(GetOpponent(team))[0, col].planted.ChangeStats(2, 0);
    }

}
