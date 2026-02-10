using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkingViolet : Card
{

	protected override IEnumerator OnThisPlay()
	{
		
		yield return new WaitForSeconds(1);
        for (int i = -1; i <= 1; i++)
        {
            if (col + i < 0 || col + i > 4) continue;
            for (int r = 0; r < Tile.ROWS; r++) if (Tile.GetTeamTiles(GetOpponent(team))[r, col + i].HasRevealedPlanted() && Tile.GetTeamTiles(GetOpponent(team))[r, col + i].planted.untrickable == 0)
            {
                Tile.GetTeamTiles(GetOpponent(team))[r, col + i].planted.ChangeStats(-2, 0);
                if (Tile.GetTeamTiles(GetOpponent(team))[r, col + i].planted.atk <= 0) Tile.GetTeamTiles(GetOpponent(team))[r, col + i].planted.Destroy();
            }
        }
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
