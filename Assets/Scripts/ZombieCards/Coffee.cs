using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coffee : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return Glow();
		for (int row = 0; row < 2; row++)
		{
			for (int col = 0; col < 5; col++)
			{
				if (Tile.GetTeamTiles(team)[row, col].HasRevealedPlanted())
				{
					Tile.GetTeamTiles(team)[row, col].planted.ChangeStats(1, 1);
					Tile.GetTeamTiles(team)[row, col].planted.frenzy += 1;
                }
			}
		}
		yield return base.OnThisPlay();
	}

}
