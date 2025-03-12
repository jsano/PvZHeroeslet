using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LurchForLunch : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		yield return Tile.zombieTiles[row, col].planted.Attack();
		yield return base.OnThisPlay();
	}

	public override bool IsValidTarget(BoxCollider2D bc)
	{
		Tile t = bc.GetComponent<Tile>();
		if (t == null) return false;
		Card c = t.planted;
		if (c == null) return false;
		if (c.team == Team.Zombie) return true;
		return false;
	}

}
