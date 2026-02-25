using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunStrike : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		for (int i = 0; i < Tile.ROWS; i++) for (int j = 0; j < Tile.COLUMNS; j++) if (Tile.GetTeamTiles(team)[i, j].HasRevealedPlanted())
				{
					Tile.GetTeamTiles(team)[i, j].planted.strikethrough += 1;
					GameManager.Instance.removeStrikethrough.Add(Tile.GetTeamTiles(team)[i, j].planted);
				}
		yield return GameManager.Instance.GainHandCard(team, AllCards.RandomTrick());
		yield return base.OnThisPlay();
	}

}
