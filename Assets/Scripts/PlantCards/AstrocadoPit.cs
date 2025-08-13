using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstrocadoPit : Card
{

	protected override IEnumerator OnTurnStart()
	{
		Tile.plantTiles[row, col].Unplant();
		yield return Glow();
		Card c = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Astrocado")]);
		Tile.plantTiles[row, col].Plant(c);
		Destroy(gameObject);
	}

}
