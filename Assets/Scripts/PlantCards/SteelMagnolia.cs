using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SteelMagnolia : Card
{

	protected override IEnumerator OnThisPlay()
	{
        yield return Glow();

		for (int i = 1; i >= -1; i--)
		{
            if (col + i < 0 || col + i > 4) continue;
            for (int r = 0; r < Tile.ROWS; r++)
            {
                if (Tile.GetTeamTiles(team)[r, col + i].HasRevealedPlanted()) Tile.GetTeamTiles(team)[r, col + i].planted.ChangeStats(0, 2);
            }
		}

		yield return base.OnThisPlay();
	}

}