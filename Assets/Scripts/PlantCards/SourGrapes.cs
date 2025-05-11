using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SourGrapes : Card
{

	protected override IEnumerator OnThisPlay()
	{
		List<Damagable> targets = new();
		for (int col = 0; col < 5; col++)
		{
			if (Tile.zombieTiles[0, col].planted != null) targets.Add(Tile.zombieTiles[0, col].planted);
		}
		yield return AttackFXs(targets);

		foreach (Damagable d in targets) StartCoroutine(d.ReceiveDamage(1, this));
		yield return base.OnThisPlay();
	}

}
