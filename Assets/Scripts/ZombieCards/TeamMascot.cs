using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamMascot : Card
{

	protected override IEnumerator OnTurnStart()
	{
		yield return new WaitForSeconds(1);
		for (int col = 0; col < 5; col++) {
			Card c = Tile.zombieTiles[0, col].planted;
			if (Tile.zombieTiles[0, col].HasRevealedPlanted() && c.tribes.Contains(Tribe.Sports))
			{
				c.RaiseAttack(1);
				c.Heal(1, true);
			}
		}
	}

}
