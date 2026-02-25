using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstrocadoPit : Card
{

	protected override IEnumerator OnTurnStart()
	{
		Tile.GetTeamTiles(team)[row, col].Unplant();
		yield return Glow();
		Card c = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Astrocado")]);
		Tile.GetTeamTiles(team)[row, col].Plant(c);
		Destroy(gameObject);
	}

}
