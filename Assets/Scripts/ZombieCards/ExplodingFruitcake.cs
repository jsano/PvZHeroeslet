using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingFruitcake : Card
{

	protected override IEnumerator OnThisPlay()
	{
        yield return new WaitForSeconds(1);
		yield return Tile.plantTiles[row, col].planted.ReceiveDamage(5, this);
        yield return GameManager.Instance.GainHandCard(Team.Plant, AllCards.RandomFromTribe((Tribe.Fruit, Tribe.Fruit)));
        yield return base.OnThisPlay();
	}

	public override bool IsValidTarget(BoxCollider2D bc)
	{
		if (!base.IsValidTarget(bc)) return false;
		Tile t = bc.GetComponent<Tile>();
		if (t != null)
		{
			if (t.HasRevealedPlanted() && t.planted.team == Team.Plant) return true;
			return false;
		}
		return false;
	}

}
