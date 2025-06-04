using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GadgetScientist : Card
{

	protected override IEnumerator OnThisPlay()
	{
		for (int i = 0; i < 5; i++)
		{
			if (Tile.zombieTiles[0, i].HasRevealedPlanted() && Tile.zombieTiles[0, i].planted.tribes.Contains(Tribe.Science)) yield return Tile.zombieTiles[0, i].planted.BonusAttack();
		}
		yield return base.OnThisPlay();
	}

}
