using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShroomForTwo : Card
{

	protected override IEnumerator OnThisPlay()
	{
        if (Tile.CanPlantInCol(col, Tile.plantTiles, true, false))
        {
            yield return new WaitForSeconds(1);
            Card card = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Puff-shroom")]).GetComponent<Card>();
            Tile.plantTiles[1, col].Plant(card);
        }

		yield return base.OnThisPlay();
	}

}
