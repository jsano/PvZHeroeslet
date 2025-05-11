using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryBomb : Card
{
	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		for (int i = -1; i <= 1; i++)
		{
			if (col + i < 0 || col + i > 4) continue;
			if (Tile.zombieTiles[0, col + i].HasRevealedPlanted()) StartCoroutine(Tile.zombieTiles[0, col + i].planted.ReceiveDamage(4, this));
		}
		yield return base.OnThisPlay();
	}

	public override bool IsValidTarget(BoxCollider2D bc)
	{
		Tile t = bc.GetComponent<Tile>();
		if (t != null) return true;
		return false;
	}

}
