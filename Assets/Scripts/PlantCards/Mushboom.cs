using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushboom : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		if (Tile.zombieTiles[0, col].HasRevealedPlanted()) StartCoroutine(Tile.zombieTiles[row, col].planted.ReceiveDamage(2, this));
		if (Tile.CanPlantInCol(col, Tile.plantTiles, false, false)) {
			Card card = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Poison Mushroom")]).GetComponent<Card>();
			Tile.plantTiles[0, col].Plant(card);
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