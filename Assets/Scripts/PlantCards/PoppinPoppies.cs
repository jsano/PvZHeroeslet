using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoppinPoppies : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);

		List<Card> todo = new();
		for (int i = -1; i <= 1; i++)
		{
            if (col + i < 0 || col + i > 4) continue;
            
			if (Tile.CanPlantInCol(col + i, Tile.plantTiles, true, false))
			{
				Card card = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Lil' Buddy")]).GetComponent<Card>();
				Tile.plantTiles[1, col + i].Plant(card);
				todo.Add(card);
			}
		}

		yield return base.OnThisPlay();
	}

}
