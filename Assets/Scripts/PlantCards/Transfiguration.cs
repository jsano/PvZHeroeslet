using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transfiguration : Card
{

	protected override IEnumerator OnTurnEnd()
	{
		Tile.GetTeamTiles(team)[row, col].Unplant(true);
		yield return Glow();
		yield return SyncRandomChoiceAcrossNetwork(AllCards.RandomFromCost(Team.A, (cost + 1, cost + 1), true) + "");
        Card c = Instantiate(AllCards.Instance.cards[int.Parse(GameManager.Instance.GetShuffledList()[0])]);
        FinalStats fs = new(int.Parse(GameManager.Instance.shuffledLists[^1][0]));
        fs.abilities += "fig";
		c.sourceFS = fs;
        Tile.GetTeamTiles(team)[row, col].Plant(c);
        Destroy(gameObject);
	}

}
