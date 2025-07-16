using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelOfBarrels : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		Tile.zombieTiles[row, col].planted.deadly += 1;
		yield return GameManager.Instance.GainHandCard(team, AllCards.RandomFromTribe((Tribe.Barrel, Tribe.Barrel)));
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
