using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaterToss : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		Card card = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Hothead")]).GetComponent<Card>();
		Tile.GetTeamTiles(team)[row, col].Plant(card);
        yield return base.OnThisPlay();
	}

	public override bool IsValidTarget(BoxCollider2D bc)
	{
		Tile t = bc.GetComponent<Tile>();
		if (t == null) return false;
        if (!t.isPlayerTile) return false;
        if (Tile.CanPlantInCol(t.col, Tile.GetTeamTiles(team), true, false)) return true;
		return false;
	}

}