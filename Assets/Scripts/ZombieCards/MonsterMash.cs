using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMash : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		for (int i = 0; i < 2; i++) for (int j = 0; j < 5; j++) if (Tile.zombieTiles[i, j].HasRevealedPlanted()) Tile.zombieTiles[i, j].planted.ChangeStats(2, 2);
		yield return base.OnThisPlay();
	}

}
