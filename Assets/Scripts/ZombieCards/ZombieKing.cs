using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieKing : Card
{

	protected override IEnumerator OnTurnEnd()
	{
        List<Tile> locations = new();
        for (int row = 0; row < 2; row++) for (int col = 0; col < 5; col++)
        {
            if (Tile.GetTeamTiles(team)[row, col].HasRevealedPlanted() && Tile.GetTeamTiles(team)[row, col].planted != this) locations.Add(Tile.GetTeamTiles(team)[row, col]);
        }
        if (locations.Count > 0)
        {
            yield return Glow();
            Tile chosen0 = locations[Random.Range(0, locations.Count)];
            yield return SyncRandomChoiceAcrossNetwork(chosen0.row + " - " + chosen0.col);
            int r = int.Parse(GameManager.Instance.GetShuffledList()[0]);
            int c = int.Parse(GameManager.Instance.GetShuffledList()[1]);
            Destroy(Tile.GetTeamTiles(team)[r, c].planted.gameObject);
            Tile.GetTeamTiles(team)[r, c].Unplant();
            Card card = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Knight of the Living Dead")]).GetComponent<Card>();
            Tile.GetTeamTiles(team)[r, c].Plant(card);
        }
        yield return base.OnTurnEnd();
    }

}
