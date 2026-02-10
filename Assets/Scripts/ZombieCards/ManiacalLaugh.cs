using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManiacalLaugh : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		Tile.GetTeamTiles(team)[row, col].planted.ChangeStats(5, 5);
		Tile.GetTeamTiles(team)[row, col].planted.frenzy += 1;
        yield return base.OnThisPlay();
	}

	public override bool IsValidTarget(BoxCollider2D bc)
	{
        if (!base.IsValidTarget(bc)) return false;
        Tile t = bc.GetComponent<Tile>();
		if (t == null) return false;
		if (t.HasRevealedPlanted() && t.planted.team == team) return true;
		return false;
	}

}
