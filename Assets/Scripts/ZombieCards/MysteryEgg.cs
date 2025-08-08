using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryEgg : Card
{

	public override IEnumerator OnZombieTricks()
	{
		Tile.zombieTiles[row, col].Unplant(true);
		yield return new WaitForSeconds(1);
        yield return SyncRandomChoiceAcrossNetwork(AllCards.RandomFromCost(Team.Zombie, (0, 1, 2), true) + "");
        Card c = Instantiate(AllCards.Instance.cards[int.Parse(GameManager.Instance.GetShuffledList()[0])]);
        Tile.zombieTiles[row, col].Plant(c);
		yield return null;
		Destroy(gameObject);
	}

}
