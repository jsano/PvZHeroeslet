using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CliquePeas : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		int id = AllCards.NameToID("Clique Peas");
		GameManager.Instance.ShuffleIntoDeck(team, new() { id, id });
		GameManager.Instance.cliquePeas += 1;
		for (int i = 0; i < 2; i++) for (int j = 0; j < 5; j++) if (Tile.plantTiles[i, j].HasRevealedPlanted() && AllCards.InstanceToPrefab(Tile.plantTiles[i, j].planted).name == "Clique Peas")
				{
					Tile.plantTiles[i, j].planted.ChangeStats(1, 1);
				}
		yield return base.OnThisPlay();
	}

}
