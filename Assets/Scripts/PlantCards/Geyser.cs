using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Geyser : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		StartCoroutine(GameManager.Instance.GetTeamHero(team).Heal(4));
		for (int i = 0; i < Tile.ROWS; i++) for (int j = 0; j < Tile.COLUMNS; j++) if (Tile.GetTeamTiles(team)[i, j].HasRevealedPlanted()) StartCoroutine(Tile.GetTeamTiles(team)[i, j].planted.Heal(4));
		yield return base.OnThisPlay();
	}

}
