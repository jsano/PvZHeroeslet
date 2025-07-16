using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveMistake : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		Tile.zombieTiles[row, col].planted.Bounce();
		yield return GameManager.Instance.DrawCard(team);
		yield return base.OnThisPlay();
	}

	public override bool IsValidTarget(BoxCollider2D bc)
	{
        if (!base.IsValidTarget(bc)) return false;
        Tile t = bc.GetComponent<Tile>();
		if (t == null) return false;
		if (t.planted != null && t.planted.team == Team.Zombie && t.planted.gravestone) return true;
		return false;
	}

}