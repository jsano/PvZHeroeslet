using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelOfDeadbeards : Card
{

	protected override IEnumerator OnCardDeath(Card died)
	{
		if (died == this)
		{
			GameManager.Instance.DisableHandCards();
			yield return new WaitForSeconds(1);
			for (int i = 0; i < 2; i++) for (int j = 0; j < 5; j++)
				{
					if (Tile.plantTiles[i, j].planted != null) StartCoroutine(Tile.plantTiles[i, j].planted.ReceiveDamage(1, this));
                    if (Tile.zombieTiles[i, j].planted != null) StartCoroutine(Tile.zombieTiles[i, j].planted.ReceiveDamage(1, this));
                }
            Card card = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Captain Deadbeard")]).GetComponent<Card>();
			Tile.zombieTiles[row, col].planted = null;
            Tile.zombieTiles[row, col].Plant(card);
        }
		yield return base.OnCardDeath(died);
	}

}
