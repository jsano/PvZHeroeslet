using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transfiguration : Card
{

	protected override IEnumerator OnTurnEnd()
	{
		Tile.plantTiles[row, col].Unplant(true);
		yield return new WaitForSeconds(1);
		if (GameManager.Instance.team == team)
		{
            /*int newCard = AllCards.RandomFromCost(Team.Plant, (cost + 1, cost + 1), true);
			FinalStats fs = new(newCard);
			fs.abilities += "fig";
			GameManager.Instance.PlayCardRpc(fs, row, col);*/
            GameManager.Instance.StoreRpc(AllCards.RandomFromCost(Team.Plant, (cost + 1, cost + 1), true) + "");
        }
        yield return new WaitUntil(() => GameManager.Instance.shuffledList != null);
        Card c = Instantiate(AllCards.Instance.cards[int.Parse(GameManager.Instance.shuffledList[0])]);
        FinalStats fs = new(int.Parse(GameManager.Instance.shuffledList[0]));
        fs.abilities += "fig";
		c.sourceFS = fs;
        Tile.plantTiles[row, col].Plant(c);
        Destroy(gameObject);
	}

}
