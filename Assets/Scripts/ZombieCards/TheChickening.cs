using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheChickening : Card
{
	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		for (int i = 0; i < 2; i++) for (int j = 0; j < 5; j++) if (Tile.GetTeamTiles(GetOpponent(team))[i, j].HasRevealedPlanted() && Tile.GetTeamTiles(GetOpponent(team))[i, j].planted.untrickable == 0) StartCoroutine(Tile.GetTeamTiles(GetOpponent(team))[i, j].planted.ReceiveDamage(2, this));
		yield return base.OnThisPlay();
	}

}
