using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SunflowerSeed : Card
{

	protected override IEnumerator OnCardDeath(Tuple<Card, Card> died)
	{
		if (died.Item1 == this)
		{
            Tile.plantTiles[row, col].Unplant(true);
            yield return new WaitForSeconds(1);
			Card c = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Sunflower")]);
			Tile.plantTiles[row, col].Plant(c);
			Destroy(gameObject);
        }
	}

}
