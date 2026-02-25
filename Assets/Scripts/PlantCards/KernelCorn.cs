using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KernelCorn : Card
{

	protected override IEnumerator OnThisPlay()
	{
		List<Damagable> targets = new();
        for (int i = 0; i < Tile.ROWS; i++) for (int col = 0; col < Tile.COLUMNS; col++)
		{
			if (Tile.GetTeamTiles(GetOpponent(team))[i, col].planted != null) targets.Add(Tile.GetTeamTiles(GetOpponent(team))[i, col].planted);
		}
        yield return Glow();
        yield return AttackFXs(targets);

		foreach (Damagable d in targets) StartCoroutine(d.ReceiveDamage(4, this));
		yield return base.OnThisPlay();
	}

}
