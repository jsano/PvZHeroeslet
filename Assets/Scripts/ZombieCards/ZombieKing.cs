using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieKing : Card
{

	protected override IEnumerator OnTurnEnd()
	{
		yield return new WaitForSeconds(1);
        List<int> locations = new();
        for (int col = 0; col < 5; col++)
        {
            if (Tile.zombieTiles[0, col].planted != null && !Tile.zombieTiles[0, col].planted.gravestone && Tile.zombieTiles[0, col].planted != this) locations.Add(col);
        }
        int chosen = Random.Range(0, locations.Count);
        Destroy(Tile.zombieTiles[0, chosen].planted.gameObject);
        Tile.zombieTiles[0, chosen].planted = null;
        Card card = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Knight of the Living Dead")]).GetComponent<Card>();
        Tile.zombieTiles[0, chosen].Plant(card);
    }

}
