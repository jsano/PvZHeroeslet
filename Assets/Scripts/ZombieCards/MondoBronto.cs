using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MondoBronto : Card
{

	protected override IEnumerator OnCardDraw(Team team)
	{
		if (team == this.team)
		{
			yield return new WaitForSeconds(1);
			ChangeStats(1, 1);
			for (int i = 0; i < 2; i++) if (Tile.plantTiles[i, col].planted != null) Tile.plantTiles[i, col].planted.Destroy();
        }
		yield return base.OnCardDraw(team);
	}

}
