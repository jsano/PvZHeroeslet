using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBaseAlpha : Card
{

    protected override IEnumerator OnThisPlay()
    {
        if (Tile.zombieTiles[0, col].HasRevealedPlanted())
        {
            Tile.zombieTiles[0, col].planted.strikethrough += 1;
            Tile.zombieTiles[0, col].planted.deadly += 1;
        }
        yield return base.OnThisPlay();
    }

    protected override IEnumerator OnCardPlay(Card played)
    {
        if (played.type == Type.Unit && played.team == Team.Zombie && played.col == col)
        {
            played.strikethrough += 1;
            played.deadly += 1;
        }
        yield return base.OnCardPlay(played);
    }

    protected override IEnumerator OnCardMoved(Card moved)
    {
        if (moved.oldCol == col && moved.col != col && moved.team == Team.Zombie)
        {
            moved.strikethrough -= 1;
            moved.deadly -= 1;
        }
        if (moved.oldCol != col && moved.col == col && moved.team == Team.Zombie)
        {
            moved.strikethrough += 1;
            moved.deadly += 1;
        }
        yield return base.OnCardMoved(moved);
    }

    protected override IEnumerator OnCardDeath(Tuple<Card, Card> died)
    {
        if (died.Item1 == this) if (Tile.zombieTiles[0, col].HasRevealedPlanted())
            {
                Tile.zombieTiles[0, col].planted.strikethrough -= 1;
                Tile.zombieTiles[0, col].planted.deadly -= 1;
            }
        yield return base.OnCardDeath(died);
    }

}
