using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aloesaurus : Card
{

	protected override IEnumerator OnCardDraw(Team team)
	{
		if (team == this.team)
		{
            yield return Glow();
            StartCoroutine(GameManager.Instance.GetTeamHero(team).Heal(1));
			for (int i = 0; i < Tile.ROWS; i++) for (int j = 0; j < Tile.COLUMNS; j++) if (Tile.GetTeamTiles(team)[i, j].HasRevealedPlanted()) StartCoroutine(Tile.GetTeamTiles(team)[i, j].planted.Heal(1));
		}
		yield return base.OnCardDraw(team);
	}

}
