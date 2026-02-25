using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdmiralNavyBean : Card
{

	protected override IEnumerator OnCardPlay(Card played)
	{
		if (played != this && played.tribes.Contains(Tribe.Bean) && played.team == team)
		{
			yield return Glow();
            yield return AttackFX(Tile.GetTeamHeroTiles(GetOpponent(team))[col]);
            yield return Tile.GetTeamHeroTiles(GetOpponent(team))[col].ReceiveDamage(2, this);
		}
		yield return base.OnCardPlay(played);
	}

}
