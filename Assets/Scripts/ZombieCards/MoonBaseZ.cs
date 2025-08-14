using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonBaseZ : Card
{

    protected override IEnumerator OnThisPlay()
    {
        if (Tile.zombieTiles[0, col].HasRevealedPlanted()) Tile.zombieTiles[0, col].planted.overshoot = Math.Max(Tile.zombieTiles[0, col].planted.baseOvershoot, 3);
        yield return base.OnThisPlay();
    }

    protected override void OnCardPlayImmediate(Card played)
    {
        if (played.type == Type.Unit && played.team == Team.Zombie && played.col == col)
        {
            played.overshoot = Math.Max(Tile.zombieTiles[0, col].planted.baseOvershoot, 3);
        }
    }

    protected override IEnumerator OnCardMoved(Card moved)
    {
        if (moved.oldCol == col && moved.col != col && moved.team == Team.Zombie)
        {
            moved.overshoot = Math.Max(Tile.zombieTiles[0, col].planted.baseOvershoot, 0);
        }
        if (moved.oldCol != col && moved.col == col && moved.team == Team.Zombie)
        {
            moved.overshoot = Math.Max(Tile.zombieTiles[0, col].planted.baseOvershoot, 3);
        }
        yield return base.OnCardMoved(moved);
    }

    protected override IEnumerator OnCardDeath(Tuple<Card, Card> died)
    {
        if (died.Item1 == this) if (Tile.zombieTiles[0, col].HasRevealedPlanted()) Tile.zombieTiles[0, col].planted.overshoot = Math.Max(Tile.zombieTiles[0, col].planted.baseOvershoot, 0);
        yield return base.OnCardDeath(died);
    }

    void OnDestroy()
    {
        if (died) return;
        if (Tile.zombieTiles[0, col].HasRevealedPlanted()) Tile.zombieTiles[0, col].planted.overshoot = Math.Max(Tile.zombieTiles[0, col].planted.baseOvershoot, 0);
    }

}
