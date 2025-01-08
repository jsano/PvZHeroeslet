using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowPea : Card
{

	protected override IEnumerator OnCardAttack(Card source)
	{
		if (source == this)
		{
			if (Tile.zombieTiles[0, col].planted != null) yield return Tile.zombieTiles[0, col].planted.Freeze();
		}
		yield return null;
	}

}

