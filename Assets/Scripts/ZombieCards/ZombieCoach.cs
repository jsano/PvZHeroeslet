using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieCoach : Card
{

	protected override IEnumerator OnThisPlay()
	{
        yield return new WaitForSeconds(1);

        for (int j = 0; j < 5; j++) if (Tile.zombieTiles[0, j].HasRevealedPlanted() && Tile.zombieTiles[0, j].planted.tribes.Contains(Tribe.Sports))
            Tile.zombieTiles[0, j].planted.ToggleInvulnerability(true);

        yield return base.OnThisPlay();
	}

}
