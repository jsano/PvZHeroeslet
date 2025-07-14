using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BogOfEnlightenment : Card
{

    protected override IEnumerator OnThisPlay()
    {
        for (int i = 0; i < 2; i++) if (Tile.plantTiles[i, col].planted != null && Tile.plantTiles[i, col].planted.amphibious) Tile.plantTiles[i, col].planted.ChangeStats(2, 0);
        if (Tile.zombieTiles[0, col].HasRevealedPlanted() && !Tile.zombieTiles[0, col].planted.amphibious) Tile.zombieTiles[0, col].planted.ChangeStats(-2, 0);
        yield return base.OnThisPlay();
    }

    protected override IEnumerator OnCardPlay(Card played)
    {
        if (played.type == Type.Unit && played.team == Team.Plant && played.col == col && played.amphibious) played.ChangeStats(2, 0);
        if (played.type == Type.Unit && played.team == Team.Zombie && played.col == col && !played.amphibious) played.ChangeStats(-2, 0);
        yield return base.OnCardPlay(played);
    }

    protected override IEnumerator OnCardMoved(Card moved)
    {
        if (moved.oldCol == col && moved.col != col && moved.team == Team.Plant && moved.amphibious) moved.ChangeStats(-2, 0);
        if (moved.oldCol == col && moved.col != col && moved.team == Team.Zombie && !moved.amphibious) moved.ChangeStats(2, 0);
        
        if (moved.oldCol != col && moved.col == col && moved.team == Team.Plant && moved.amphibious) moved.ChangeStats(2, 0);
        if (moved.oldCol != col && moved.col == col && moved.team == Team.Zombie && !moved.amphibious) moved.ChangeStats(-2, 0);
        yield return base.OnCardMoved(moved);
    }

    protected override IEnumerator OnCardDeath(Tuple<Card, Card> died)
    {
        if (died.Item1 == this)
        {
            for (int i = 0; i < 2; i++) if (Tile.plantTiles[i, col].planted != null && Tile.plantTiles[i, col].planted.amphibious) Tile.plantTiles[i, col].planted.ChangeStats(-2, 0);
            if (Tile.zombieTiles[0, col].HasRevealedPlanted() && !Tile.zombieTiles[0, col].planted.amphibious) Tile.zombieTiles[0, col].planted.ChangeStats(2, 0);
        }
        yield return base.OnCardDeath(died);
    }

}
