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
            yield return Glow();
            if (col - 1 >= 0 && Tile.GetTeamTiles(team)[1, col - 1].planted == null)
			{
				Card card = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Swabbie")]).GetComponent<Card>();
				Tile.GetTeamTiles(team)[1, col - 1].Plant(card);
			}
            if (col + 1 <= 4 && Tile.GetTeamTiles(team)[1, col + 1].planted == null)
            {
                Card card = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Swabbie")]).GetComponent<Card>();
                Tile.GetTeamTiles(team)[1, col + 1].Plant(card);
            }
        }
		yield return base.OnCardDeath(died);
	}

}
