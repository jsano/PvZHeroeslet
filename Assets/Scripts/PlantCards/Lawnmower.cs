using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lawnmower : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		Tile.GetTeamTiles(GetOpponent(team))[row, col].planted.Destroy();
		yield return base.OnThisPlay();
	}

	public override bool IsValidTarget(BoxCollider2D bc)
	{
        if (!base.IsValidTarget(bc)) return false;
        Tile t = bc.GetComponent<Tile>();
		if (t == null || t.col == 0 || t.col == 4) return false;
		if (!t.HasRevealedPlanted()) return false;
		if (t.planted.team == GetOpponent(GameManager.Instance.team)) return true;
		return false;
	}

}