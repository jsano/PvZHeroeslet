using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squash : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		Tile.zombieTiles[row, col].planted.Destroy();
		yield return base.OnThisPlay();
	}

	public override bool IsValidTarget(BoxCollider2D bc)
	{
		Tile t = bc.GetComponent<Tile>();
		if (t == null) return false;
		if (!t.HasRevealedPlanted()) return false;
		if (t.planted.team == Team.Zombie) return true;
		return false;
	}

}