using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryEgg : Card
{

	public override IEnumerator OnZombieTricks()
	{
		Tile.plantTiles[row, col].Unplant(true);
		yield return new WaitForSeconds(1);
		if (GameManager.Instance.team == team)
		{
			int newCard = AllCards.RandomFromCost(Team.Zombie, (0, 1, 2), true);
			GameManager.Instance.PlayCardRpc(new FinalStats(newCard), row, col);
		}
		yield return null;
		Destroy(gameObject);
	}

}
