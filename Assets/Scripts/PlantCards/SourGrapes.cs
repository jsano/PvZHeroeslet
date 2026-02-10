using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SourGrapes : Card
{

	protected override IEnumerator OnThisPlay()
	{
		List<Damagable> targets = new();
        for (int r = 0; r < Tile.ROWS; r++) for (int col = 0; col < Tile.COLUMNS; col++)
		{
			if (Tile.GetTeamTiles(GetOpponent(team))[r, col].planted != null) targets.Add(Tile.GetTeamTiles(GetOpponent(team))[r, col].planted);
		}
        yield return Glow();
        yield return AttackFXs(targets);

		foreach (Damagable d in targets) StartCoroutine(d.ReceiveDamage(1, this));
		yield return base.OnThisPlay();
	}

}
