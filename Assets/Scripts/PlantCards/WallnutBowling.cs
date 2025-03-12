using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallnutBowling : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		int heroDmg = 0;
		for (int i = 1; i < 4; i++)
		{
			if (Tile.zombieTiles[0, i].planted != null) Tile.zombieTiles[0, i].planted.ReceiveDamage(6);
			else heroDmg += 6;

            Card card = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Wall-nut")]).GetComponent<Card>();
			Tile.plantTiles[1, i].Plant(card);
		}
		GameManager.Instance.zombieHero.ReceiveDamage(heroDmg);

		yield return base.OnThisPlay();
	}

}
