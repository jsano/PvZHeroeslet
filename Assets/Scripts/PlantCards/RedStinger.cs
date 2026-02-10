using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedStinger : Card
{

	protected override IEnumerator OnThisPlay()
	{
		if (Tile.GetTeamTiles(team)[1, col].planted != null && row == 0)
		{
			yield return Glow();
			SetStats(7, 3);
		}
		yield return base.OnThisPlay();
	}

}
