using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeweedSector : Card
{

	public override IEnumerator BeforeCombat()
	{
		if (Tile.GetTeamTiles(GetOpponent(team))[0, col].HasRevealedPlanted() || Tile.GetTeamTiles(GetOpponent(team))[1, col].HasRevealedPlanted())
		{
			yield return new WaitForSeconds(1);
			if (Tile.GetTeamTiles(GetOpponent(team))[0, col].HasRevealedPlanted()) StartCoroutine(Tile.GetTeamTiles(GetOpponent(team))[0, col].planted.ReceiveDamage(2, this));
            if (Tile.GetTeamTiles(GetOpponent(team))[1, col].HasRevealedPlanted()) StartCoroutine(Tile.GetTeamTiles(GetOpponent(team))[1, col].planted.ReceiveDamage(2, this));
        }
		yield return base.BeforeCombat();
	}

}
