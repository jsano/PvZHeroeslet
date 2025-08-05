using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PearCub : Card
{

	protected override IEnumerator OnCardDeath(Tuple<Card, Card> died)
	{
		if (died.Item1 == this)
		{
            Tile.plantTiles[row, col].Unplant(true);
            yield return new WaitForSeconds(1);
			Card c1 = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Grizzly Pear")]);
			Tile.plantTiles[row, col].Plant(c1);

            for (int row = 0; row < 2; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    Card c = Tile.plantTiles[row, col].planted;
                    if (c != null && c.tribes.Contains(Tribe.Fruit))
                    {
                        c.ChangeStats(1, 1);
                    }
                }
            }

            Destroy(gameObject);
        }
	}

}
