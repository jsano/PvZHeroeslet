using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eyespore : Card
{

	protected override IEnumerator Fusion(Card parent)
	{
        if (Tile.zombieTiles[0, col].HasRevealedPlanted())
        {
            yield return new WaitForSeconds(1);
            Tile.zombieTiles[0, col].planted.Destroy();
        }
        yield return base.Fusion(parent);
    }

}
