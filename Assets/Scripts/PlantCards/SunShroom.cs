using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunShroom : Card
{

	protected override IEnumerator OnTurnStart()
	{
        Tile.plantTiles[row, col].Unplant(true);
		yield return GameManager.Instance.UpdateRemaining(1, team);
        Card c = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Sunnier-shroom")]);
        Tile.plantTiles[row, col].Plant(c);
        Destroy(gameObject);
    }

}
