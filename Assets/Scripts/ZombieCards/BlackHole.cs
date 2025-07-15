using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : Card
{

    protected override IEnumerator OnThisPlay()
    {
        for (int i = 0; i < 2; i++) if (Tile.plantTiles[i, col].planted != null) Tile.plantTiles[i, col].planted.ChangeStats(-1, 0);
        yield return base.OnThisPlay();
    }

    protected override IEnumerator OnCardPlay(Card played)
    {
        if (played.type == Type.Unit && played.team == Team.Plant && played.col == col)
        {
            played.ChangeStats(-1, 0);
        }
        if (played.type == Type.Unit && played.team == Team.Plant && played.col != col && Tile.CanPlantInCol(col, Tile.plantTiles, played.teamUp, played.amphibious))
        {
            yield return new WaitForSeconds(1);
            played.Move(played.row, col);
        }
        yield return base.OnCardPlay(played);
    }

    protected override IEnumerator OnCardMoved(Card moved)
    {
        if (moved.oldCol == col && moved.col != col && moved.team == Team.Plant)
        {
            moved.ChangeStats(1, 0);
        }
        if (moved.oldCol != col && moved.col == col && moved.team == Team.Plant)
        {
            moved.ChangeStats(-1, 0);
        }
        yield return base.OnCardMoved(moved);
    }

    protected override IEnumerator OnCardDeath(Tuple<Card, Card> died)
    {
        if (died.Item1 == this)
        {
            for (int i = 0; i < 2; i++) if (Tile.plantTiles[i, col].planted != null) Tile.plantTiles[i, col].planted.ChangeStats(1, 0);
        }
        yield return base.OnCardDeath(died);
    }

}
