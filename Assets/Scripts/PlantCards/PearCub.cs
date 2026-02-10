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
            Tile.GetTeamTiles(team)[row, col].Unplant(true);
            yield return Glow();
            Card c1 = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Grizzly Pear")]);
			Tile.GetTeamTiles(team)[row, col].Plant(c1);

            for (int row = 0; row < Tile.ROWS; row++)
            {
                for (int col = 0; col < Tile.COLUMNS; col++)
                {
                    Card c = Tile.GetTeamTiles(team)[row, col].planted;
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
