using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizzardLizard : Card
{

	protected override IEnumerator OnThisPlay()
	{
		if (evolved)
		{
			List<Damagable> targets = new();
			for (int i = 0; i < 2; i++) for (int j = 0; j < 5; j++)
			{
				if (Tile.plantTiles[i, j].planted != null) targets.Add(Tile.plantTiles[i, j].planted);
			}
            yield return Glow();
            yield return AttackFXs(targets);
			foreach (Damagable c in targets) StartCoroutine(c.ReceiveDamage(3, this));
		}
		yield return base.OnThisPlay();
	}

}