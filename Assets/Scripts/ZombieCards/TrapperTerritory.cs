using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapperTerritory : Card
{

	public override IEnumerator BeforeCombat()
	{
		List<Damagable> targets = new();
		for (int i = 0; i < 2; i++) if (Tile.GetTeamTiles(GetOpponent(team))[i, col].HasRevealedPlanted()) targets.Add(Tile.GetTeamTiles(GetOpponent(team))[i, col].planted);
		if (targets.Count > 0)
		{
            yield return Glow();
            foreach (Damagable c in targets) StartCoroutine(c.ReceiveDamage(1, this));
		}
		yield return base.BeforeCombat();
	}

}
