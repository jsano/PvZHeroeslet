using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelRoller : Card
{

	protected override IEnumerator OnCardDeath(Tuple<Card, Card> died)
	{
		if (died.Item1 == this)
		{
			yield return new WaitForSeconds(1);
            if (col - 1 >= 0 && Tile.zombieTiles[0, col - 1].planted == null)
			{
				Card card = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Swabbie")]).GetComponent<Card>();
				Tile.zombieTiles[row, col - 1].Plant(card);
			}
            if (col + 1 <= 4 && Tile.zombieTiles[0, col + 1].planted == null)
            {
                Card card = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Swabbie")]).GetComponent<Card>();
                Tile.zombieTiles[row, col + 1].Plant(card);
            }
        }
		yield return base.OnCardDeath(died);
	}

}
