using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NutSignal : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		Card card = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Wall-nut")]).GetComponent<Card>();
		Tile.plantTiles[row, col].Plant(card);
		GameManager.Instance.DrawCard(team);
        yield return base.OnThisPlay();
	}

	public override bool IsValidTarget(BoxCollider2D bc)
	{
		Tile t = bc.GetComponent<Tile>();
		if (t == null) return false;
		if (!t.isPlantTile) return false;
        if (Tile.CanPlantInCol(t.col, Tile.plantTiles, true, false)) return true;
		return false;
	}

}