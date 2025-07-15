using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeThroughTime : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		Tile.zombieTiles[row, col].planted.ToggleInvulnerability(true);
		yield return GameManager.Instance.GainHandCard(team, AllCards.RandomFromTribe((Tribe.History, Tribe.History)));
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
