using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapperTerritory : Card
{

	public override IEnumerator BeforeCombat()
	{
		List<Damagable> targets = new();
		for (int i = 0; i < 2; i++) if (Tile.plantTiles[i, col].HasRevealedPlanted()) targets.Add(Tile.plantTiles[i, col].planted);
		if (targets.Count > 0)
		{
			yield return new WaitForSeconds(1);
			foreach (Damagable c in targets) StartCoroutine(c.ReceiveDamage(1, this));
		}
		yield return base.BeforeCombat();
	}

}
