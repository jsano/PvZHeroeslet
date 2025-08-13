using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AtomicBombegranate : Card
{

	protected override IEnumerator OnCardDeath(Tuple<Card, Card> died)
	{
		if (died.Item1 == this)
		{
			Tile.plantTiles[row, col].Unplant();
            yield return Glow();
			if (col > 0 && Tile.CanPlantInCol(col - 1, Tile.plantTiles, false, false))
			{
				Card c = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Seedling")]);
				Tile.plantTiles[0, col - 1].Plant(c);
			}
            if (col < 4 && Tile.CanPlantInCol(col + 1, Tile.plantTiles, false, false))
			{
                Card c = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Seedling")]);
                Tile.plantTiles[0, col + 1].Plant(c);
            }
			Card c1 = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Seedling")]);
            Tile.plantTiles[row, col].Plant(c1);
        }
		yield return base.OnCardDeath(died);
	}

}
