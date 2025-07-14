using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area22 : Card
{

    protected override IEnumerator OnThisPlay()
    {
        if (Tile.zombieTiles[0, col].planted != null)
        {
            Tile.zombieTiles[0, col].planted.ChangeStats(2, 2);
            Tile.zombieTiles[0, col].planted.frenzy += 1;
        }
        yield return base.OnThisPlay();
    }

    protected override IEnumerator OnCardPlay(Card played)
    {
        if (played.type == Type.Unit && played.team == Team.Zombie && played.col == col)
        {
            played.ChangeStats(2, 2);
            played.frenzy += 1;
        }
        yield return base.OnCardPlay(played);
    }

    protected override IEnumerator OnCardMoved(Card moved)
    {
        if (moved.oldCol == col && moved.col != col && moved.team == Team.Zombie)
        {
            moved.ChangeStats(-2, -2);
            moved.frenzy -= 1;
        }
        if (moved.oldCol != col && moved.col == col && moved.team == Team.Zombie)
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
            if (Tile.zombieTiles[0, col].planted != null)
            {
                Tile.zombieTiles[0, col].planted.ChangeStats(-2, -2);
                Tile.zombieTiles[0, col].planted.frenzy -= 1;
            }
        }
        yield return base.OnCardDeath(died);
    }

}
