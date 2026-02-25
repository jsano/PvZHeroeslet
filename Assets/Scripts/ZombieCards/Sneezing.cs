using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sneezing : Card
{

	protected override IEnumerator OnThisPlay()
	{
		
		yield return Glow();
        for (int i = 0; i < 2; i++) for (int j = 0; j < 5; j++)
        {
            if (Tile.GetTeamTiles(GetOpponent(team))[i, j].HasRevealedPlanted()) Tile.GetTeamTiles(GetOpponent(team))[i, j].planted.ChangeStats(-1, -1);
        }
        yield return base.OnThisPlay();
	}

}
