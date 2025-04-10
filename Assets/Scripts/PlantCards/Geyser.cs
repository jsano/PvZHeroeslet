using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Geyser : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		GameManager.Instance.plantHero.Heal(4);
		for (int i = 0; i < 2; i++) for (int j = 0; j < 5; j++) if (Tile.plantTiles[i, j].HasRevealedPlanted()) Tile.plantTiles[i, j].planted.Heal(4);
		yield return base.OnThisPlay();
	}

}
