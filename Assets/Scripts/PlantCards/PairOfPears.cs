using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PairOfPears : Card
{

	protected override IEnumerator OnThisPlay()
	{
        if (Tile.CanPlantInCol(col, Tile.plantTiles, true, false))
        {
            yield return Glow();
            Card card = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Pear Pal")]).GetComponent<Card>();
            Tile.plantTiles[1, col].Plant(card);
        }

		yield return base.OnThisPlay();
	}

}
