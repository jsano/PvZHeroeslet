using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformationStation : Card
{

	protected override IEnumerator OnTurnStart()
	{
		if (Tile.zombieTiles[0, col].HasRevealedPlanted())
		{
			Card c = Tile.zombieTiles[0, col].planted;
			Tile.zombieTiles[0, col].Unplant(true);
			yield return new WaitForSeconds(1);
			if (GameManager.Instance.team == team)
			{
				int newCard = AllCards.RandomFromCost(Team.Zombie, (c.cost + 1, c.cost + 1), true);
				GameManager.Instance.PlayCardRpc(new FinalStats(newCard), 0, col);
			}
			yield return null;
			Destroy(c.gameObject);
		}
		yield return base.OnTurnStart();
	}

}
