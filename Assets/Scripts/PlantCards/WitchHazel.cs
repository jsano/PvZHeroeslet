using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchHazel : Card
{

	protected override IEnumerator OnTurnEnd()
	{
        List<int> locations = new();
        for (int col = 0; col < 5; col++)
        {
            if (Tile.zombieTiles[0, col].HasRevealedPlanted() && Tile.zombieTiles[0, col].planted != this) locations.Add(col);
        }
        if (GameManager.Instance.team == team)
        {
            int chosen0 = locations[Random.Range(0, locations.Count)];
            if (locations.Count > 0) GameManager.Instance.StoreRpc(chosen0 + "");
        }
        if (locations.Count > 0)
        {
            yield return new WaitForSeconds(1);
            int chosen = int.Parse(GameManager.Instance.shuffledList[0]);
            Tile.zombieTiles[0, chosen].planted.Destroy();
            if (Tile.CanPlantInCol(chosen, Tile.plantTiles, true, false))
            {
                Card card = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Puff-shroom")]).GetComponent<Card>();
                Tile.plantTiles[1, chosen].Plant(card);
            }
        }
        yield return base.OnTurnEnd();
    }

}
