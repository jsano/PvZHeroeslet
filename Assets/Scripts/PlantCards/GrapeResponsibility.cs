using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapeResponsibility : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		Tile.plantTiles[row, col].planted.ChangeStats(0, Tile.plantTiles[row, col].planted.HP);
        yield return base.OnThisPlay();
	}

	public override bool IsValidTarget(BoxCollider2D bc)
	{
        if (!base.IsValidTarget(bc)) return false;
        Tile t = bc.GetComponent<Tile>();
		if (t == null) return false;
		Card c = t.planted;
		if (c == null) return false;
		if (c.team == Team.Plant) return true;
		return false;
	}

}
