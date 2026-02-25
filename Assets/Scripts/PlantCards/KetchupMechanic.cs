using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KetchupMechanic : Card
{
    private int count = 0;

	protected override IEnumerator OnThisPlay()
	{
        for (int i = 0; i < Tile.ROWS; i++) for (int j = 0; j < Tile.COLUMNS; j++)
		{
            count += Tile.GetTeamTiles(GetOpponent(team))[i, j].HasRevealedPlanted() ? 1 : 0;
        }
		if (count > 0)
		{
			yield return Glow();
			ChangeStats(count, count);
			yield return GameManager.Instance.GetTeamHero(team).Heal(count);
		}
		yield return base.OnThisPlay();
	}

}
