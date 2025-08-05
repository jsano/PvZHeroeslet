using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SnakeGrass : Card
{

	protected override IEnumerator OnTurnStart()
	{
		if (col < 4 && Tile.CanPlantInCol(col + 1, Tile.plantTiles, false, true))
		{
            yield return new WaitForSeconds(1);
            Card c = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Snake Grass")]);
            Tile.plantTiles[0, col + 1].Plant(c);
        }
		yield return base.OnTurnStart();
	}

}
