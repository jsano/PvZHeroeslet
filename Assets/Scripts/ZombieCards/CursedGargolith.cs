using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursedGargolith : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return Glow();
		int amount = GameManager.Instance.plantHero.StealBlock(2);
		GameManager.Instance.zombieHero.StealBlock(-amount);
		yield return base.OnThisPlay();
	}

    protected override IEnumerator OnTurnEnd()
    {
        yield return Glow();
        for (int col = 0; col < 5; col++) if (Tile.zombieTiles[0, col].planted != null && Tile.zombieTiles[0, col].planted.tribes.Contains(Tribe.Gargantuar))
				Tile.zombieTiles[0, col].planted.Hide();
        yield return base.OnTurnEnd();
    }

}
