using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaiseStats : Card
{

	public int atkAmount;
	public int HPAmount;
	public Team targetTeam;

	protected override IEnumerator OnThisPlay()
	{
		var tiles = targetTeam == Team.Plant ? Tile.plantTiles : Tile.zombieTiles;
		yield return new WaitForSeconds(1);
		tiles[row, col].planted.ChangeStats(atkAmount, HPAmount);
		yield return base.OnThisPlay();
	}

	public override bool IsValidTarget(BoxCollider2D bc)
	{
        if (!base.IsValidTarget(bc)) return false;
        Tile t = bc.GetComponent<Tile>();
		if (t == null) return false;
		Card c = t.planted;
		if (c == null) return false;
		if (c.team == targetTeam) return true;
		return false;
	}

}
