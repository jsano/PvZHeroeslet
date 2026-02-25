using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoingViral : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
        for (int r = 0; r < Tile.ROWS; r++) for (int j = 0; j < 5; j++) if (Tile.GetTeamTiles(team)[r, j].HasRevealedPlanted())
			{
				Tile.GetTeamTiles(team)[r, j].planted.ChangeStats(1, 1);
				Tile.GetTeamTiles(team)[r, j].planted.frenzy += 1;
            }
		int id = AllCards.NameToID("Going Viral");
		GameManager.Instance.ShuffleIntoDeck(team, new() { id, id, id });
		yield return GameManager.Instance.DrawCard(team);
		yield return base.OnThisPlay();
	}

}
