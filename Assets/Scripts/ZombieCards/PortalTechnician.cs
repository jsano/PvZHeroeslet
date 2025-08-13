using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PortalTechnician : Card
{

	protected override IEnumerator OnCardDeath(Tuple<Card, Card> died)
	{
		if (died.Item1 == this)
		{
            Tile.zombieTiles[row, col].Unplant(true);
            yield return Glow();
            yield return SyncRandomChoiceAcrossNetwork(AllCards.RandomFromCost(team, (4, 5, 6, 7, 8, 9, 10, 11), true) + "");
            Card c = Instantiate(AllCards.Instance.cards[int.Parse(GameManager.Instance.GetShuffledList()[0])]);
            Tile.zombieTiles[row, col].Plant(c);
        }
		yield return base.OnCardDeath(died);
	}

}
