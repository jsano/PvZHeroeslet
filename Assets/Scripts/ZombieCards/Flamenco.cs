using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamenco : Card
{

	protected override IEnumerator OnThisPlay()
	{
		int dmg = 0;
        for (int r = 0; r < Tile.ROWS; r++) for (int i = 0; i < 5; i++)
		{
			if (Tile.GetTeamTiles(team)[r, i].planted != null && Tile.GetTeamTiles(team)[r, i].planted.tribes.Contains(Tribe.Dancing)) dmg += 2;
		}
        yield return Glow();
        yield return AttackFX(GameManager.Instance.GetTeamHero(GetOpponent(team)));
		yield return GameManager.Instance.GetTeamHero(GetOpponent(team)).ReceiveDamage(dmg, this);
		yield return base.OnThisPlay();
	}

}
