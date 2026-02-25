using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AerobicsInstructor : Card
{

	protected override IEnumerator OnTurnStart()
	{
        yield return Glow();
        for (int r = 0; r < Tile.ROWS; r++) for (int col = 0; col < 5; col++) {
			Card c = Tile.GetTeamTiles(team)[r, col].planted;
			if (Tile.GetTeamTiles(team)[r, col].HasRevealedPlanted() && c.tribes.Contains(Tribe.Dancing))
			{
				c.ChangeStats(2, 0);
			}
		}
	}

}
