using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatTerrain : Card
{

    public int atkBuff;
    public int HPBuff;
    public Team targetTeam;

    protected override IEnumerator OnThisPlay()
    {
        for (int i = 0; i < 2; i++) if (Tile.GetTeamTiles(targetTeam)[i, col].planted != null) Tile.GetTeamTiles(targetTeam)[i, col].planted.ChangeStats(atkBuff, HPBuff);
        yield return base.OnThisPlay();
    }

    protected override void OnCardPlayImmediate(Card played)
    {
        if (played.type == Type.Unit && played.team == targetTeam && played.col == col)
        {
            played.ChangeStats(atkBuff, HPBuff);
        }
    }

    protected override IEnumerator OnCardMoved(Card moved)
    {
        if (moved.oldCol == col && moved.col != col && moved.team == targetTeam)
        {
            moved.ChangeStats(-atkBuff, -HPBuff);
        }
        if (moved.oldCol != col && moved.col == col && moved.team == targetTeam)
        {
            moved.ChangeStats(atkBuff, HPBuff);
        }
        yield return base.OnCardMoved(moved);
    }

    protected override IEnumerator OnCardDeath(Tuple<Card, Card> died)
    {
        if (died.Item1 == this)
        {
            for (int i = 0; i < 2; i++) if (Tile.GetTeamTiles(targetTeam)[i, col].planted != null) Tile.GetTeamTiles(targetTeam)[i, col].planted.ChangeStats(-atkBuff, -HPBuff);
        }
        yield return base.OnCardDeath(died);
    }

    void OnDestroy()
    {
        if (died) return;
        for (int i = 0; i < 2; i++) if (Tile.GetTeamTiles(targetTeam)[i, col].planted != null) Tile.GetTeamTiles(targetTeam)[i, col].planted.ChangeStats(-atkBuff, -HPBuff);
    }

}
