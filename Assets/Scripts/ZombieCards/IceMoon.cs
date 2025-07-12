using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceMoon : Card
{

    protected override IEnumerator OnThisPlay()
    {
        for (int i = 0; i < 2; i++) if (Tile.plantTiles[i, col].planted != null)
        {
            Tile.plantTiles[i, col].planted.Freeze();
        }
        if (Tile.zombieTiles[0, col].HasRevealedPlanted()) Tile.zombieTiles[0, col].planted.strikethrough += 1;
        yield return base.OnThisPlay();
    }

    protected override IEnumerator OnCardPlay(Card played)
    {
        if (played.type == Type.Unit && played.team == Team.Zombie && played.col == col)
        {
            played.strikethrough += 1;
        }
        yield return base.OnCardPlay(played);
    }

    protected override IEnumerator OnCardMoved(Card moved)
    {
        if (moved.oldCol == col && moved.col != col && moved.team == Team.Zombie)
        {
            moved.strikethrough -= 1;
        }
        if (moved.oldCol != col && moved.col == col && moved.team == Team.Zombie)
        {
            moved.strikethrough += 1;
        }
        yield return base.OnCardMoved(moved);
    }

    protected override IEnumerator OnCardDeath(Card died)
    {
        Debug.Log("here");
        if (died == this) if (Tile.zombieTiles[0, col].HasRevealedPlanted()) Tile.zombieTiles[0, col].planted.strikethrough -= 1;
        yield return base.OnCardDeath(died);
    }

}
