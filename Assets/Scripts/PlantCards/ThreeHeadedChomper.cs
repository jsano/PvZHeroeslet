using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeHeadedChomper : Card
{

	protected override IEnumerator OnTurnEnd()
	{
		yield return Glow();
        for (int i = -1; i <= 1; i++)
        {
            if (col + i < 0 || col + i >= 5) continue;
            for (int r = 0; r < Tile.ROWS; r++) if (Tile.GetTeamTiles(GetOpponent(team))[r, col + i].HasRevealedPlanted()) Tile.GetTeamTiles(GetOpponent(team))[r, col + i].planted.Destroy();
        }
        yield return base.OnTurnEnd();
    }

}
