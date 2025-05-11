using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveBuster : Card
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
		if (t.planted != null && !t.HasRevealedPlanted()) return true;
		return false;
	}

}