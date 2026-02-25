using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombotDinotronicMechasaur : Card
{

	protected override IEnumerator OnCardDraw(Team team)
    {
        if (team == this.team)
        {
            List<Tile> columns = new();
            for (int row = 0; row < 2; row++) for (int col = 0; col < 5; col++)
            {
                if (Tile.CanPlantInCol(col, Tile.GetTeamTiles(team), false, true)) columns.Add(Tile.GetTeamTiles(team)[row, col]);
            }
            if (columns.Count > 0)
            {
                yield return Glow();
                Tile chosen = columns[UnityEngine.Random.Range(0, columns.Count)];
                yield return SyncRandomChoiceAcrossNetwork(chosen.row + " - " + chosen.col + " - " + AllCards.RandomFromTribe((Tribe.History, Tribe.History), true));
                Card c = Instantiate(AllCards.Instance.cards[int.Parse(GameManager.Instance.GetShuffledList()[2])]).GetComponent<Card>();
                Tile.GetTeamTiles(team)[int.Parse(GameManager.Instance.GetShuffledList()[0]), int.Parse(GameManager.Instance.GetShuffledList()[1])].Plant(c);
            }
        }
        yield return base.OnCardDraw(team);
	}

}
