using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoingViral : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		for (int j = 0; j < 5; j++) if (Tile.zombieTiles[0, j].HasRevealedPlanted())
			{
				Tile.zombieTiles[0, j].planted.ChangeStats(1, 1);
				Tile.zombieTiles[0, j].planted.frenzy += 1;
            }
		int id = AllCards.NameToID("Going Viral");
		GameManager.Instance.ShuffleIntoDeck(team, new() { id, id, id });
		yield return GameManager.Instance.DrawCard(team);
		yield return base.OnThisPlay();
	}

}
