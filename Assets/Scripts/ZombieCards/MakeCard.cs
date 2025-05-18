using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeCard : Card
{

	public Card toMake;

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		Card card = Instantiate(toMake).GetComponent<Card>();
		Tile.zombieTiles[row, col].Plant(card);
        yield return base.OnThisPlay();
	}

	public override bool IsValidTarget(BoxCollider2D bc)
	{
		Tile t = bc.GetComponent<Tile>();
		if (t == null) return false;
		if (t.isPlantTile) return false;
        if (t.row == 0 && t.planted == null && Tile.CanPlantInCol(t.col, Tile.zombieTiles, false, toMake.amphibious)) return true;
		return false;
	}

}