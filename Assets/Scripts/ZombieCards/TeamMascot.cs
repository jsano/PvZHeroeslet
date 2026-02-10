using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamMascot : Card
{

	protected override IEnumerator OnTurnStart()
	{
		yield return Glow();
        for (int row = 0; row < 2; row++) for (int col = 0; col < 5; col++) {
			Card c = Tile.GetTeamTiles(team)[row, col].planted;
			if (Tile.GetTeamTiles(team)[row, col].HasRevealedPlanted() && c.tribes.Contains(Tribe.Sports))
			{
				c.ChangeStats(1, 1);
			}
		}
	}

}
