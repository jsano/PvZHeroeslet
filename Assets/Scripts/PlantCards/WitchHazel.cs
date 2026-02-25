using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchHazel : Card
{

	protected override IEnumerator OnTurnEnd()
	{
        List<Tile> locations = new();
        for (int row = 0; row < Tile.ROWS; row++) for (int col = 0; col < Tile.COLUMNS; col++)
        {
            if (Tile.GetTeamTiles(GetOpponent(team))[row, col].HasRevealedPlanted() && !Tile.GetTeamTiles(GetOpponent(team))[row, col].planted.died) locations.Add(Tile.GetTeamTiles(GetOpponent(team))[row, col]);
        }
        if (locations.Count > 0)
        {
            yield return Glow();
            Tile random = locations[UnityEngine.Random.Range(0, locations.Count)];
            yield return SyncRandomChoiceAcrossNetwork(random.row + " - " + random.col);
            int chosen = int.Parse(GameManager.Instance.GetShuffledList()[1]);
            Tile.GetTeamTiles(GetOpponent(team))[int.Parse(GameManager.Instance.GetShuffledList()[0]), chosen].planted.Destroy();
            if (Tile.CanPlantInCol(chosen, Tile.GetTeamTiles(team), true, false))
            {
                Card card = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Puff-shroom")]).GetComponent<Card>();
                Tile.GetTeamTiles(team)[1, chosen].Plant(card);
            }
        }
        yield return base.OnTurnEnd();
    }

}
