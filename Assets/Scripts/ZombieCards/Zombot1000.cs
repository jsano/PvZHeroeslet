using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombot1000 : Card
{

	protected override IEnumerator OnThisPlay()
	{
        yield return Glow();
        for (int i = 0; i < 5; i++)
		{
			if (Tile.GetTeamTiles(GetOpponent(team))[0, i].planted != null) Tile.GetTeamTiles(GetOpponent(team))[0, i].planted.Destroy();
            if (Tile.GetTeamTiles(GetOpponent(team))[1, i].planted != null) Tile.GetTeamTiles(GetOpponent(team))[1, i].planted.Destroy();
        }
		yield return base.OnThisPlay();
	}

}
