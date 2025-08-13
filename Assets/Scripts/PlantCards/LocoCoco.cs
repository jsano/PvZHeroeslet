using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocoCoco : Card
{

	protected override IEnumerator OnThisPlay()
	{
        yield return Glow();

		for (int i = 1; i >= -1; i -= 2)
		{
            if (col + i < 0 || col + i > 4) continue;
            
			if (Tile.CanPlantInCol(col + i, Tile.plantTiles, true, false))
			{
				Card card = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Wall-nut")]).GetComponent<Card>();
				Tile.plantTiles[1, col + i].Plant(card);
			}
		}

		if (evolved)
		{
            yield return Glow();
            for (int row = 0; row < 2; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    Card c = Tile.plantTiles[row, col].planted;
                    if (c != null && c.atk == 0) c.ChangeStats(3, 0);
                }
            }
        }

		yield return base.OnThisPlay();
	}

}
