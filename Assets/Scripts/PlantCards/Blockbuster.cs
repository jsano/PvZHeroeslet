using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blockbuster : Card
{

	protected override IEnumerator OnThisPlay()
	{
		if (evolved)
		{
			yield return Glow();
			for (int i = -1; i <= 1; i++)
			{
				if (col + i < 0 || col + i > 4) continue;
				if (Tile.GetTeamTiles(GetOpponent(team))[0, col + i].planted != null && Tile.GetTeamTiles(GetOpponent(team))[0, col + i].planted.gravestone) Tile.GetTeamTiles(GetOpponent(team))[0, col + i].planted.Destroy();
			}
		}
		yield return base.OnThisPlay();
	}

}