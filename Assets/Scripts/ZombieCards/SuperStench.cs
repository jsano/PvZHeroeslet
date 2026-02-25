using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperStench : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
        for (int row = 0; row < 2; row++) for (int j = 0; j < 5; j++) if (Tile.GetTeamTiles(team)[row, j].HasRevealedPlanted()) Tile.GetTeamTiles(team)[row, j].planted.deadly += 1;
		yield return GameManager.Instance.DrawCard(team);
		yield return base.OnThisPlay();
	}

}
