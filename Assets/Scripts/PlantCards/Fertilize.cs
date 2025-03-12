using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fertilize : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		Tile.plantTiles[row, col].planted.RaiseAttack(3);
		Tile.plantTiles[row, col].planted.Heal(3, true);
		yield return base.OnThisPlay();
	}

	public override bool IsValidTarget(BoxCollider2D bc)
	{
		Tile t = bc.GetComponent<Tile>();
		if (t == null) return false;
		Card c = t.planted;
		if (c == null) return false;
		if (c.team == Team.Plant) return true;
		return false;
	}

}
