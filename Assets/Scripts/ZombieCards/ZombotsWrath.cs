using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombotsWrath : Card
{
	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		int count = 0;
		for (int i = 0; i < 2; i++) for (int j = 0; j < 5; j++) if (Tile.zombieTiles[i, j].HasRevealedPlanted()) count += 1;
		yield return Tile.plantTiles[row, col].planted.ReceiveDamage(count + 1, this);
		yield return base.OnThisPlay();
	}

	public override bool IsValidTarget(BoxCollider2D bc)
	{
        if (!base.IsValidTarget(bc)) return false;
        Tile t = bc.GetComponent<Tile>();
		if (t == null) return false;
		if (t.planted != null && t.planted.team == Team.Plant) return true;
		return false;
	}

}
