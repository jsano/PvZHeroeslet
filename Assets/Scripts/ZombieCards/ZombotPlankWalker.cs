using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombotPlankWalker : Card
{

	protected override IEnumerator OnThisPlay()
	{
        List<Tile> columns = new();
        for (int row = 0; row < 2; row++) for (int col = 0; col < 5; col++)
        {
            if (Tile.CanPlantInCol(col, Tile.GetTeamTiles(team), false, true)) columns.Add(Tile.GetTeamTiles(team)[row, col]);
        }
        for (int n = columns.Count - 1; n > 0; n--)
        {
            int k = UnityEngine.Random.Range(0, n + 1);
            var temp = columns[n];
            columns[n] = columns[k];
            columns[k] = temp;
        }
        if (columns.Count > 0)
        {
            yield return Glow();
            string s = "";
            for (int i = 0; i < Mathf.Min(2, columns.Count); i++)
            {
                int c = AllCards.RandomFromTribe((Tribe.Pirate, Tribe.Pirate), true, columns[i].col == 4);
                while (c == AllCards.NameToID("Zombot Plank Walker")) c = AllCards.RandomFromTribe((Tribe.Pirate, Tribe.Pirate), true, columns[i].col == 4);
                s += columns[i].row + " - " + columns[i].col + " - " + c + " - ";
            }
            yield return SyncRandomChoiceAcrossNetwork(s);
            for (int i = 0; i < GameManager.Instance.GetShuffledList().Count - 1; i += 3)
            {
                Card c = Instantiate(AllCards.Instance.cards[int.Parse(GameManager.Instance.GetShuffledList()[i + 2])]);
                Tile.GetTeamTiles(team)[int.Parse(GameManager.Instance.GetShuffledList()[i]), int.Parse(GameManager.Instance.GetShuffledList()[i + 1])].Plant(c);
            }
        }
        yield return base.OnThisPlay();
	}

}
