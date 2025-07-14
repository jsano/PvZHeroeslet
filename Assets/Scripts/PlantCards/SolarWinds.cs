using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarWinds : Card
{

	protected override IEnumerator OnTurnEnd()
	{
		if (!Tile.zombieTiles[0, col].HasRevealedPlanted() && Tile.CanPlantInCol(col, Tile.plantTiles, true, false))
		{
			yield return new WaitForSeconds(1);
			Card card = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Sunflower")]).GetComponent<Card>();
			Tile.plantTiles[1, col].Plant(card);	
		}
		yield return base.OnTurnEnd();
	}

}