using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombot1000 : Card
{

	protected override IEnumerator OnThisPlay()
	{
        yield return Glow();
        for (int i = 0; i < 5; i++)
		{
			if (Tile.plantTiles[0, i].planted != null) Tile.plantTiles[0, i].planted.Destroy();
            if (Tile.plantTiles[1, i].planted != null) Tile.plantTiles[1, i].planted.Destroy();
        }
		yield return base.OnThisPlay();
	}

}
