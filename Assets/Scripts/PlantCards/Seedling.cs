using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seedling : Card
{

	protected override IEnumerator OnTurnStart()
	{
		Tile.plantTiles[row, col].Unplant(true);
		yield return Glow();
        yield return SyncRandomChoiceAcrossNetwork(AllCards.RandomFromCost(Team.Plant, (0, 1, 2, 3, 4, 5, 6), true) + "");
        Card c = Instantiate(AllCards.Instance.cards[int.Parse(GameManager.Instance.GetShuffledList()[0])]);
        Tile.plantTiles[row, col].Plant(c);
        yield return null;
		Destroy(gameObject);
	}

}
