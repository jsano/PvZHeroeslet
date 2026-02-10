using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockout : Card
{

    protected override IEnumerator OnThisPlay()
    {
        for (int i = 0; i < 2; i++) if (Tile.GetTeamTiles(GetOpponent(team))[i, col].planted != null && Tile.GetTeamTiles(GetOpponent(team))[i, col].planted.atk <= 3) Tile.GetTeamTiles(GetOpponent(team))[i, col].planted.Destroy();
        yield return base.OnThisPlay();
    }

    public override bool IsValidTarget(BoxCollider2D bc)
    {
        if (!base.IsValidTarget(bc)) return false;
        Tile t = bc.GetComponent<Tile>();
        if (t != null && t.isTerrainTile) return true;
        return false;
    }

}
