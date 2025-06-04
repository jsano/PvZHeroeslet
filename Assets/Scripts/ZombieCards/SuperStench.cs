using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperStench : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		for (int j = 0; j < 5; j++) if (Tile.zombieTiles[0, j].HasRevealedPlanted()) Tile.zombieTiles[0, j].planted.deadly += 1;
		yield return GameManager.Instance.DrawCard(team);
		yield return base.OnThisPlay();
	}

}
