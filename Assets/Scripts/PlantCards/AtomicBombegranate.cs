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
			Tile.GetTeamTiles(team)[row, col].Unplant();
            yield return Glow();
			if (col > 0 && Tile.CanPlantInCol(col - 1, Tile.GetTeamTiles(team), false, false))
			{
				Card c = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Seedling")]);
				Tile.GetTeamTiles(team)[0, col - 1].Plant(c);
			}
            if (col < 4 && Tile.CanPlantInCol(col + 1, Tile.GetTeamTiles(team), false, false))
			{
                Card c = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Seedling")]);
                Tile.GetTeamTiles(team)[0, col + 1].Plant(c);
            }
			Card c1 = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Seedling")]);
            Tile.GetTeamTiles(team)[row, col].Plant(c1);
        }
		yield return base.OnCardDeath(died);
	}

}
