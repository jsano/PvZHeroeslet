using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelociRadishPackmate : Card
{

	protected override IEnumerator OnThisPlay()
	{
        if (Tile.CanPlantInCol(col, Tile.plantTiles, true, false))
        {
            yield return Glow();
            Card card = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Veloci-Radish Hatchling")]).GetComponent<Card>();
            Tile.plantTiles[1, col].Plant(card);
        }

		yield return base.OnThisPlay();
	}

    protected override IEnumerator OnCardDraw(Team team)
    {
        if (team == this.team)
        {
            yield return Glow();
            ChangeStats(1, 0);
        }
        yield return base.OnCardDraw(team);
    }

}
