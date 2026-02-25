using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireworks : Card
{

	protected override IEnumerator OnThisPlay()
	{
        List<Damagable> targets = new();
            
		for (int i = 0; i < 2; i++) for (int j = 0; j < 5; j++)
			{
				if (Tile.playerTiles[i, j].planted != null) targets.Add(Tile.playerTiles[i, j].planted);
                if (Tile.opponentTiles[i, j].planted != null) targets.Add(Tile.opponentTiles[i, j].planted);
            }
        yield return Glow();
        yield return AttackFXs(targets);
		foreach (Damagable d in targets) StartCoroutine(d.ReceiveDamage(1, this));
		yield return base.OnThisPlay();
	}

}
