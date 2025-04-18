using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallnutBowling : Card
{

	protected override IEnumerator OnThisPlay()
	{
        yield return new WaitForSeconds(1);

        int heroDmg = 0;
		for (int i = 3; i >= 1; i--)
		{
			if (Tile.zombieTiles[0, i].planted != null) yield return Tile.zombieTiles[0, i].planted.ReceiveDamage(6, this);
			else heroDmg += 6;
		}
		yield return GameManager.Instance.zombieHero.ReceiveDamage(heroDmg, this);

        for (int i = 1; i <= 3; i++)
        {
            if (Tile.CanPlantInCol(i, Tile.plantTiles, true, false))
            {
                Card card = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Wall-nut")]);
                Tile.plantTiles[1, i].Plant(card);
            }
        }

        yield return base.OnThisPlay();
	}

}
