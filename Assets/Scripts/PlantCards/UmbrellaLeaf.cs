using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UmbrellaLeaf : Card
{

    protected override IEnumerator OnThisPlay()
    {
        for (int i = 0; i < 2; i++) for (int j = -1; j <= 1; j++)
        {
            if (col + j < 0 || col + j > 4) continue;
            if (Tile.plantTiles[i, col + j].planted != null && Tile.plantTiles[i, col + j].planted != this) Tile.plantTiles[i, col + j].planted.untrickable += 1;
        }
        yield return base.OnThisPlay();
    }

    protected override IEnumerator OnCardPlay(Card played)
    {
        if (played != this && played.type == Type.Unit && played.team == Team.Plant && HereAndNextDoor(played.col))
        {
            played.untrickable += 1;
        }
        yield return base.OnCardPlay(played);
    }

    protected override IEnumerator OnCardMoved(Card moved)
    {
        if (moved != this && HereAndNextDoor(moved.oldCol) && !HereAndNextDoor(moved.col) && moved.team == Team.Plant)
        {
            moved.untrickable -= 1;
        }
        if (moved != this && !HereAndNextDoor(moved.oldCol) && HereAndNextDoor(moved.col) && moved.team == Team.Plant)
        {
            moved.untrickable += 1;
        }
        yield return base.OnCardMoved(moved);
    }

    protected override IEnumerator OnCardDeath(Tuple<Card, Card> died)
    {
        if (died.Item1 == this) for (int i = 0; i < 2; i++) for (int j = -1; j <= 1; j++)
                {
                    if (col + j < 0 || col + j > 4) continue;
                    if (Tile.plantTiles[i, col + j].planted != null && Tile.plantTiles[i, col + j].planted != this) Tile.plantTiles[i, col + j].planted.untrickable -= 1;
                }
        yield return base.OnCardDeath(died);
    }

    void OnDestroy() // When this is an evolution source
    {
        if (!died) for (int i = 0; i < 2; i++) for (int j = -1; j <= 1; j++)
                {
                    if (col + j < 0 || col + j > 4) continue;
                    if (Tile.plantTiles[i, col + j].planted != null && Tile.plantTiles[i, col + j].planted != this) Tile.plantTiles[i, col + j].planted.untrickable -= 1;
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
