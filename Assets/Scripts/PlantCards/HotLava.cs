using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotLava : Card
{

	public override IEnumerator BeforeCombat()
	{
		List<Damagable> targets = new();
		for (int i = 0; i < Tile.ROWS; i++) if (Tile.playerTiles[i, col].HasRevealedPlanted()) targets.Add(Tile.playerTiles[i, col].planted);
        for (int i = 0; i < Tile.ROWS; i++) if (Tile.opponentTiles[i, col].HasRevealedPlanted()) targets.Add(Tile.opponentTiles[i, col].planted);

        if (targets.Count > 0) {
            yield return Glow();
            foreach (Damagable d in targets) StartCoroutine(d.ReceiveDamage(1, this));
		}
		yield return base.BeforeCombat();
	}

}
