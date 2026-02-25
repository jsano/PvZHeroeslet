using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolBean : Card
{

	protected override IEnumerator OnThisPlay()
	{
        yield return Glow();
        for (int col = 0; col < Tile.COLUMNS; col++)
		{
			if (Tile.GetTeamTiles(GetOpponent(team))[0, col].planted != null && Tile.GetTeamTiles(GetOpponent(team))[0, col].planted.gravestone) Tile.GetTeamTiles(GetOpponent(team))[0, col].planted.Freeze();
		}
		yield return base.OnThisPlay();
	}

}
