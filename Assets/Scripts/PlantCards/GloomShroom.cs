using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloomShroom : Card
{

	protected override IEnumerator OnThisPlay()
	{
		if (evolved)
        {
            List<Damagable> targets = new();
            for (int i = -1; i <= 1; i++)
            {
                if (col + i < 0 || col + i > 4) continue;
                for (int r = 0; r < Tile.ROWS; r++) if (Tile.GetTeamTiles(GetOpponent(team))[r, col + i].HasRevealedPlanted()) targets.Add(Tile.GetTeamTiles(GetOpponent(team))[r, col + i].planted);
            }
            yield return Glow();
            yield return AttackFXs(targets);
            foreach (Damagable c in targets) StartCoroutine(c.ReceiveDamage(3, this));
        }
		yield return base.OnThisPlay();
	}

}
