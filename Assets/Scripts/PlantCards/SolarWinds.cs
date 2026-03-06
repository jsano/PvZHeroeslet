using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarWinds : Card
{

	protected override IEnumerator OnTurnEnd()
	{
		if (!Tile.GetTeamTiles(GetOpponent(team))[0, col].HasRevealedPlanted() && Tile.CanPlantInCol(col, Tile.GetTeamTiles(team), true, false))
		{
            yield return Glow();
            Card card = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Sunflower")]).GetComponent<Card>();
			Tile.GetTeamTiles(team)[1, col].Plant(card);	
		}
		yield return base.OnTurnEnd();
	}

}