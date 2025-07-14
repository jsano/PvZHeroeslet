using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienOoze : Card
{
	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		if (col == 0 || Tile.terrainTiles[col].planted != null) Tile.plantTiles[row, col].planted.ChangeStats(-6, -6);
		else Tile.plantTiles[row, col].planted.ChangeStats(-3, -3);
        yield return base.OnThisPlay();
	}

	public override bool IsValidTarget(BoxCollider2D bc)
	{
		Tile t = bc.GetComponent<Tile>();
		if (t == null) return false;
		if (t.planted != null && t.planted.team == Team.Plant) return true;
		return false;
	}

}
