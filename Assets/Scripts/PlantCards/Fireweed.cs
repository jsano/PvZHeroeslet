using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireweed : Card
{

	protected override IEnumerator OnThisPlay()
	{
        if (col >= 1 && col <= 3)
        {
            yield return Glow();
            Card c = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Hot Lava")]);
            Tile.terrainTiles[col].Plant(c);
        }
		yield return base.OnThisPlay();
	}

}
