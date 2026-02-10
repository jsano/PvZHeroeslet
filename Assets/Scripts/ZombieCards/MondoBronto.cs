using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MondoBronto : Card
{

	protected override IEnumerator OnCardDraw(Team team)
	{
		if (team == this.team)
		{
            yield return Glow();
            ChangeStats(1, 1);
			for (int i = 0; i < 2; i++) if (Tile.GetTeamTiles(GetOpponent(team))[i, col].planted != null) Tile.GetTeamTiles(GetOpponent(team))[i, col].planted.Destroy();
        }
		yield return base.OnCardDraw(team);
	}

}
