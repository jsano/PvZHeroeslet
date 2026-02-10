using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformationStation : Card
{

	protected override IEnumerator OnTurnStart()
	{
		if (Tile.GetTeamTiles(team)[0, col].HasRevealedPlanted())
		{
			Card c = Tile.GetTeamTiles(team)[0, col].planted;
			Tile.GetTeamTiles(team)[0, col].Unplant(true);
			yield return new WaitForSeconds(1);
            yield return SyncRandomChoiceAcrossNetwork(AllCards.RandomFromCost(Team.B, (c.cost + 1, c.cost + 1), true) + "");
            Card c1 = Instantiate(AllCards.Instance.cards[int.Parse(GameManager.Instance.GetShuffledList()[0])]);
            Tile.GetTeamTiles(team)[0, col].Plant(c1);
			Destroy(c.gameObject);
		}
		yield return base.OnTurnStart();
	}

}
