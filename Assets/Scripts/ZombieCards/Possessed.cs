using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Possessed : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		Tile.zombieTiles[row, col].planted.ChangeStats(0, 2);
		Tile.zombieTiles[row, col].planted.frenzy += 1;
        yield return base.OnThisPlay();
	}

	public override bool IsValidTarget(BoxCollider2D bc)
	{
        if (!base.IsValidTarget(bc)) return false;
        Tile t = bc.GetComponent<Tile>();
		if (t == null) return false;
		if (t.HasRevealedPlanted() && t.planted.team == Team.Zombie) return true;
		return false;
	}

}
