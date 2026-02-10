using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushboom : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		if (Tile.GetTeamTiles(GetOpponent(team))[0, col].HasRevealedPlanted() && Tile.GetTeamTiles(GetOpponent(team))[0, col].planted.untrickable == 0) StartCoroutine(Tile.GetTeamTiles(GetOpponent(team))[0, col].planted.ReceiveDamage(2, this));
		if (Tile.CanPlantInCol(col, Tile.GetTeamTiles(team), false, false)) {
			Card card = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Poison Mushroom")]).GetComponent<Card>();
			Tile.GetTeamTiles(team)[0, col].Plant(card);
		}
        yield return base.OnThisPlay();
	}

	public override bool IsValidTarget(BoxCollider2D bc)
	{
        if (!base.IsValidTarget(bc)) return false;
        Tile t = bc.GetComponent<Tile>();
		if (t != null && t.isTerrainTile) return true;
		return false;
	}

}