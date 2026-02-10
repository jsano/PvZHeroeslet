using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeedSpray : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		for (int i = 0; i < 2; i++) for (int j = 1; j < 4; j++) if (Tile.GetTeamTiles(GetOpponent(team))[i, j].HasRevealedPlanted() && Tile.GetTeamTiles(GetOpponent(team))[i, j].planted.untrickable == 0 && Tile.GetTeamTiles(GetOpponent(team))[i, j].planted.atk <= 2)
		{
			Tile.GetTeamTiles(GetOpponent(team))[i, j].planted.Destroy();
		}

		yield return base.OnThisPlay();
	}

}
