using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Devour : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		Tile.GetTeamTiles(GetOpponent(team))[row, col].planted.Destroy();
		yield return base.OnThisPlay();
	}

	public override bool IsValidTarget(BoxCollider2D bc)
	{
        if (!base.IsValidTarget(bc)) return false;
        List<BoxCollider2D> targets = new();
		int lowest = 999;
        for (int r = 0; r < Tile.ROWS; r++) for (int i = 0; i < 5; i++) if (Tile.GetTeamTiles(GetOpponent(team))[r, i].HasRevealedPlanted() && Tile.GetTeamTiles(GetOpponent(team))[r, i].planted.HP < lowest)
			{
				lowest = Tile.GetTeamTiles(GetOpponent(team))[r, i].planted.HP;
            }
        for (int r = 0; r < Tile.ROWS; r++) for (int i = 0; i < 5; i++) if (Tile.GetTeamTiles(GetOpponent(team))[r, i].HasRevealedPlanted() && Tile.GetTeamTiles(GetOpponent(team))[r, i].planted.HP == lowest)
            {
                targets.Add(Tile.GetTeamTiles(GetOpponent(team))[r, i].GetComponent<BoxCollider2D>());
            }
        if (targets.Contains(bc)) return true;
		return false;
	}

}