using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UntrickableNextDoor : Card
{

    public Team targetTeam;

    protected override IEnumerator OnThisPlay()
    {
        var tiles = targetTeam == Team.Plant ? Tile.plantTiles : Tile.zombieTiles;
        for (int i = 0; i < 2; i++) for (int j = -1; j <= 1; j++)
        {
            if (col + j < 0 || col + j > 4) continue;
            if (tiles[i, col + j].HasRevealedPlanted() && tiles[i, col + j].planted != this) tiles[i, col + j].planted.untrickable += 1;
        }
        yield return base.OnThisPlay();
    }

    protected override IEnumerator OnCardPlay(Card played)
    {
        if (played != this && played.type == Type.Unit && played.team == targetTeam && HereAndNextDoor(played.col))
        {
            played.untrickable += 1;
        }
        yield return base.OnCardPlay(played);
    }

    protected override IEnumerator OnCardMoved(Card moved)
    {
        if (moved != this && HereAndNextDoor(moved.oldCol) && !HereAndNextDoor(moved.col) && moved.team == targetTeam)
        {
            moved.untrickable -= 1;
        }
        if (moved != this && !HereAndNextDoor(moved.oldCol) && HereAndNextDoor(moved.col) && moved.team == targetTeam)
        {
            moved.untrickable += 1;
        }
        yield return base.OnCardMoved(moved);
    }

    protected override IEnumerator OnCardDeath(Tuple<Card, Card> died)
    {
        var tiles = targetTeam == Team.Plant ? Tile.plantTiles : Tile.zombieTiles;
        if (died.Item1 == this) for (int i = 0; i < 2; i++) for (int j = -1; j <= 1; j++)
                {
                    if (col + j < 0 || col + j > 4) continue;
                    if (tiles[i, col + j].HasRevealedPlanted() && tiles[i, col + j].planted != this) tiles[i, col + j].planted.untrickable -= 1;
                }
        yield return base.OnCardDeath(died);
    }

    void OnDestroy() // When this is an evolution source
    {
        var tiles = targetTeam == Team.Plant ? Tile.plantTiles : Tile.zombieTiles;
        if (!died) for (int i = 0; i < 2; i++) for (int j = -1; j <= 1; j++)
                {
                    if (col + j < 0 || col + j > 4) continue;
                    if (tiles[i, col + j].HasRevealedPlanted() && tiles[i, col + j].planted != this) tiles[i, col + j].planted.untrickable -= 1;
                }
    }

    private bool HereAndNextDoor(int c)
    {
        for (int i = -1; i <= 1; i++)
        {
            if (col + i < 0 || col + i > 4) continue;
            if (col + i == c) return true;
        }
        return false;
    }

}
