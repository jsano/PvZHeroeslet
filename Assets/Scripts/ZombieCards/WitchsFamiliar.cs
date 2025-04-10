using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchsFamiliar : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		Card card = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Zom-bats")]).GetComponent<Card>();
		Tile.zombieTiles[0, col].Plant(card);
        yield return base.OnThisPlay();
	}

	public override bool IsValidTarget(BoxCollider2D bc)
	{
		Tile t = bc.GetComponent<Tile>();
		if (t == null) return false;
        if (Tile.CanPlantInCol(t.col, Tile.zombieTiles, false, true)) return true;
		return false;
	}

}