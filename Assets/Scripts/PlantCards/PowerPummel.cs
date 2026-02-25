using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPummel : Card
{

	protected override IEnumerator OnThisPlay()
	{
        yield return new WaitForSeconds(1);

        int heroDmg = 0;
		for (int i = 3; i >= 1; i--)
		{
            for (int r = 0; r < Tile.ROWS; r++) if (Tile.GetTeamTiles(GetOpponent(team))[r, i].planted != null) yield return Tile.GetTeamTiles(GetOpponent(team))[r, i].planted.ReceiveDamage(2, this);
            if (Tile.GetTeamTiles(GetOpponent(team))[0, i].planted == null && Tile.GetTeamTiles(GetOpponent(team))[1, i].planted == null) heroDmg += 2;
		}
		yield return GameManager.Instance.GetTeamHero(GetOpponent(team)).ReceiveDamage(heroDmg, this);

        yield return base.OnThisPlay();
	}

}
