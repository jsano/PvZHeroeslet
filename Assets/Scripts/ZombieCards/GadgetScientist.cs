using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GadgetScientist : Card
{

	protected override IEnumerator OnThisPlay()
	{
        yield return Glow();
        for (int r = 0; r < Tile.ROWS; r++) for (int i = 0; i < 5; i++)
		{
			if (Tile.GetTeamTiles(team)[r, i].HasRevealedPlanted() && Tile.GetTeamTiles(team)[r, i].planted.tribes.Contains(Tribe.Science)) yield return Tile.GetTeamTiles(team)[r, i].planted.BonusAttack();
		}
		yield return base.OnThisPlay();
	}

}
