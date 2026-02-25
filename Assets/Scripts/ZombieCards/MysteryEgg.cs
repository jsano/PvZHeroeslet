using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryEgg : Card
{

	public override IEnumerator OnZombieTricks()
	{
		Tile.GetTeamTiles(team)[row, col].Unplant(true);
		yield return Glow();
        yield return SyncRandomChoiceAcrossNetwork(AllCards.RandomFromCost((0, 1, 2), true) + "");
        Card c = Instantiate(AllCards.Instance.cards[int.Parse(GameManager.Instance.GetShuffledList()[0])]);
        Tile.GetTeamTiles(team)[row, col].Plant(c);
		yield return null;
		Destroy(gameObject);
	}

}
