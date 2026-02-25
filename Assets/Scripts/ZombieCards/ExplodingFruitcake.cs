using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingFruitcake : Card
{

	protected override IEnumerator OnThisPlay()
	{
        yield return new WaitForSeconds(1);
		yield return Tile.GetTeamTiles(GetOpponent(team))[row, col].planted.ReceiveDamage(5, this);
        yield return GameManager.Instance.GainHandCard(GetOpponent(team), AllCards.RandomFromTribe((Tribe.Fruit, Tribe.Fruit)));
        yield return base.OnThisPlay();
	}

	public override bool IsValidTarget(BoxCollider2D bc)
	{
		if (!base.IsValidTarget(bc)) return false;
		Tile t = bc.GetComponent<Tile>();
		if (t != null)
		{
			if (t.HasRevealedPlanted() && t.planted.team != GameManager.Instance.team) return true;
			return false;
		}
		return false;
	}

}
