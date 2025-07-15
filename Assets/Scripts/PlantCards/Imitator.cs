using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Imitator : Card
{

	protected override IEnumerator OnCardPlay(Card played)
	{
		if (played != this && played.type == Type.Unit && played.team == team)
		{
            Tile.plantTiles[row, col].Unplant(true);
            yield return new WaitForSeconds(1);
            Card c = Instantiate(AllCards.InstanceToPrefab(played));
            Tile.plantTiles[row, col].Plant(c);
            Destroy(gameObject);
        }
    }

}
