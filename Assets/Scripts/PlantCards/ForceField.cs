using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceField : Card
{

    protected override IEnumerator OnThisPlay()
    {
        for (int i = 0; i < 2; i++) if (Tile.plantTiles[i, col].planted != null) Tile.plantTiles[i, col].planted.ToggleInvulnerability(true);
        yield return base.OnThisPlay();
    }

    protected override IEnumerator OnCardPlay(Card played)
    {
        if (played.type == Type.Unit && played.team == Team.Plant && played.col == col)
        {
            played.ToggleInvulnerability(true);
        }
        yield return base.OnCardPlay(played);
    }

    protected override IEnumerator OnCardMoved(Card moved)
    {
        if (moved.oldCol == col && moved.team == Team.Plant)
        {
            moved.ToggleInvulnerability(false);
        }
        if (moved.col == col && moved.team == Team.Plant)
        {
            moved.ToggleInvulnerability(true);
        }
        yield return base.OnCardMoved(moved);
    }

    protected override IEnumerator OnCardDeath(Card died)
    {
        if (died == this) for (int i = 0; i < 2; i++) if (Tile.plantTiles[i, col].planted != null) Tile.plantTiles[i, col].planted.ToggleInvulnerability(false);
        yield return base.OnCardDeath(died);
    }

    protected override IEnumerator OnTurnStart()
    {
        for (int i = 0; i < 2; i++) if (Tile.plantTiles[i, col].planted != null) Tile.plantTiles[i, col].planted.ToggleInvulnerability(true);
        yield return base.OnTurnStart();
    }

}
