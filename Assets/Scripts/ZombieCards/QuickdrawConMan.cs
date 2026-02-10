using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickdrawConMan : Card
{

	protected override IEnumerator OnCardDraw(Team team)
	{
		if (team != this.team)
		{
            yield return Glow();
            yield return AttackFX(Tile.GetTeamHeroTiles(GetOpponent(team))[col]);
			yield return Tile.GetTeamHeroTiles(GetOpponent(team))[col].ReceiveDamage(1, this, bullseye > 0);
        }
		yield return base.OnCardDraw(team);
	}

}
