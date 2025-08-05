using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BananaSplit : Card
{

	protected override IEnumerator OnCardDeath(Tuple<Card, Card> died)
	{
		if (died.Item1 == this)
		{
			Tile.plantTiles[row, col].Unplant();
            yield return new WaitForSeconds(1);
			if (col > 0 && Tile.CanPlantInCol(col - 1, Tile.plantTiles, false, false))
			{
				Card c = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Half-Banana")]);
				Tile.plantTiles[0, col - 1].Plant(c);
			}
            if (col < 3 && Tile.CanPlantInCol(col + 1, Tile.plantTiles, false, false))
			{
                Card c = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Half-Banana")]);
                Tile.plantTiles[0, col + 1].Plant(c);
            }
        }
		yield return base.OnCardDeath(died);
	}

}
