using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seedling : Card
{

	protected override IEnumerator OnTurnStart()
	{
		Tile.plantTiles[row, col].planted = null;
		yield return new WaitForSeconds(1);
		if (GameManager.Instance.team == team)
		{
			int newCard = AllCards.RandomFromCost(Team.Plant, (0, 1, 2, 3, 4, 5, 6), true);
			GameManager.Instance.PlayCardRpc(HandCard.MakeDefaultFS(newCard), row, col, true);
		}
		Destroy(gameObject);
	}

}
