using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toadstool : Card
{

	protected override IEnumerator OnThisPlay()
	{
		if (Tile.zombieTiles[0, col].HasRevealedPlanted() && Tile.zombieTiles[0, col].planted.atk <= 4)
		{
			yield return new WaitForSeconds(1);
			Tile.zombieTiles[0, col].planted.Destroy();
		}
		yield return base.OnThisPlay();
    }

    protected override IEnumerator OnTurnStart()
    {
        yield return GameManager.Instance.UpdateRemaining(1, team);
        yield return base.OnTurnStart();
    }

}
