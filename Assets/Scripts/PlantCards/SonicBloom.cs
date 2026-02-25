using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonicBloom : Card
{
    
	private int count = 0;

	protected override IEnumerator OnThisPlay()
	{
		for (int i = 0; i < Tile.ROWS; i++)
		{
			for (int j = 0; j < Tile.COLUMNS; j++)
			{
				count += Tile.GetTeamTiles(team)[i, j].HasRevealedPlanted() ? 1 : 0;
            }
		}
        yield return Glow();
        yield return AttackFX(Tile.GetTeamHeroTiles(GetOpponent(team))[col]);
        yield return GameManager.Instance.GetTeamHero(GetOpponent(team)).ReceiveDamage(count, this, bullseye > 0);
		yield return base.OnThisPlay();
	}

}
