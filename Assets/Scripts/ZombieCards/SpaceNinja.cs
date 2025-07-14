using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceNinja : Card
{

	private bool first = true;

	protected override IEnumerator OnCardHurt(Tuple<Damagable, Card, int, int> hurt)
	{
		if (hurt.Item2 == this && first && Tile.terrainTiles[col].planted != null)
		{
			first = false;
			List<Damagable> targets = new();
			for (int i = 0; i < 2; i++) for (int col = 0; col < 5; col++)
			{
				if (Tile.plantTiles[i, col].planted != null) targets.Add(Tile.plantTiles[i, col].planted);
			}
			yield return AttackFXs(targets);

			foreach (Damagable d in targets) StartCoroutine(d.ReceiveDamage(1, this));
		}
		
		yield return base.OnCardHurt(hurt);
	}

    protected override IEnumerator OnTurnStart()
    {
		first = true;
        yield return base.OnTurnStart();
    }

}
