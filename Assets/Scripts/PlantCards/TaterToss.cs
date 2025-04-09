using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaterToss : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		if (Tile.CanPlantInCol(col, Tile.plantTiles, true, false)) {
			Card card = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Hothead")]).GetComponent<Card>();
			Tile.plantTiles[row, col].Plant(card);
		}
        yield return base.OnThisPlay();
	}

	public override bool IsValidTarget(BoxCollider2D bc)
	{
		Tile t = bc.GetComponent<Tile>();
		if (t == null) return false;
		return true;
	}

}