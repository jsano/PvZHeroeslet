using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtinctionEvent : Card
{

    protected override IEnumerator OnThisPlay()
    {
        Card c0 = Tile.plantTiles[row, col].planted;
        yield return new WaitForSeconds(1);
        for (int i = 0; i < 2; i++) for (int j = 0; j < 5; j++)
        {
            var c = Tile.plantTiles[i, j].planted;
            if (c != null && AllCards.InstanceToPrefab(c) == AllCards.InstanceToPrefab(c0))
            {
                c.ChangeStats(-2, -2);
            }
        }
        yield return base.OnThisPlay();
    }

    public override bool IsValidTarget(BoxCollider2D bc)
    {
        if (!base.IsValidTarget(bc)) return false;
        Tile t = bc.GetComponent<Tile>();
        if (t == null) return false;
        if (t.HasRevealedPlanted() && t.planted.team == Team.Plant) return true;
        return false;
    }

}
