using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieKing : Card
{

	protected override IEnumerator OnTurnEnd()
	{
        if (GameManager.Instance.team == team)
        {
            List<int> locations = new();
            for (int col = 0; col < 5; col++)
            {
                if (Tile.zombieTiles[0, col].HasRevealedPlanted() && Tile.zombieTiles[0, col].planted != this) locations.Add(col);
            }
            int chosen0 = locations[Random.Range(0, locations.Count)];
            GameManager.Instance.StoreRpc(chosen0 + "");
        }
        yield return new WaitForSeconds(1);
        int chosen = int.Parse(GameManager.Instance.shuffledList[0]);
        Destroy(Tile.zombieTiles[0, chosen].planted.gameObject);
        Tile.zombieTiles[0, chosen].Unplant();
        Card card = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Knight of the Living Dead")]).GetComponent<Card>();
        Tile.zombieTiles[0, chosen].Plant(card);
        yield return base.OnTurnEnd();
    }

}
