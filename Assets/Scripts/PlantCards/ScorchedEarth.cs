using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorchedEarth : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
        for (int i = 0; i < Tile.ROWS; i++) for (int j = 1; j < 4; j++) if (Tile.GetTeamTiles(GetOpponent(team))[i, j].HasRevealedPlanted() && Tile.GetTeamTiles(GetOpponent(team))[i, j].planted.untrickable == 0)
			{
				Tile.GetTeamTiles(GetOpponent(team))[i, j].planted.ChangeStats(-1, -1);
            }
		yield return base.OnThisPlay();
	}

}
