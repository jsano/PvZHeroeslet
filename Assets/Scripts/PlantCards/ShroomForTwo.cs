using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShroomForTwo : Card
{

	protected override IEnumerator OnThisPlay()
	{
        if (Tile.CanPlantInCol(col, Tile.GetTeamTiles(team), true, false))
        {
            yield return Glow();
            Card card = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Puff-shroom")]).GetComponent<Card>();
            Tile.GetTeamTiles(team)[1, col].Plant(card);
        }

		yield return base.OnThisPlay();
	}

}
