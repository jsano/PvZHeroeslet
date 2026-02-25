using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursedGargolith : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return Glow();
		int amount = GameManager.Instance.GetTeamHero(GetOpponent(team)).StealBlock(2);
		GameManager.Instance.GetTeamHero(team).StealBlock(-amount);
		yield return base.OnThisPlay();
	}

    protected override IEnumerator OnTurnEnd()
    {
        yield return Glow();
        for (int r = 0; r < Tile.ROWS; r++) for (int col = 0; col < 5; col++) if (Tile.GetTeamTiles(team)[r, col].planted != null && Tile.GetTeamTiles(team)[r, col].planted.tribes.Contains(Tribe.Gargantuar))
				Tile.GetTeamTiles(team)[r, col].planted.Hide();
        yield return base.OnTurnEnd();
    }

}
