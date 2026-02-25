using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombotBattlecruiser5000 : Card
{

	protected override IEnumerator OnThisPlay()
	{
        yield return Glow();
        for (int row = 0; row < 2; row++) for (int j = 0; j < 5; j++) if (Tile.GetTeamTiles(team)[row, j].HasRevealedPlanted()) Tile.GetTeamTiles(team)[row, j].planted.ToggleInvulnerability(true);
        yield return base.OnThisPlay();
	}

}
