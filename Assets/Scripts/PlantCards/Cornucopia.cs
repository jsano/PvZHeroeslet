using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cornucopia : Card
{

	protected override IEnumerator OnThisPlay()
	{		
        List<int> locations = new();
        for (int col = 0; col < 5; col++)
        {
            if (Tile.CanPlantInCol(col, Tile.plantTiles, false, true)) locations.Add(col);
        }
        if (locations.Count > 0)
        {
            yield return Glow();
            string s = "";
            for (int i = 0; i < locations.Count; i++)
            {
                s += locations[i] + " - " + AllCards.RandomFromCost(team, (0,1,2,3,4,5,6,7,8,9,10,11,12), true, col == 4) + " - ";
            }
            yield return SyncRandomChoiceAcrossNetwork(s);
            for (int i = 0; i < GameManager.Instance.shuffledLists[^1].Count - 1; i += 2)
            {
                Card c = Instantiate(AllCards.Instance.cards[int.Parse(GameManager.Instance.GetShuffledList()[i + 1])]);
                Tile.plantTiles[0, int.Parse(GameManager.Instance.GetShuffledList()[i])].Plant(c);
            }
        }

        yield return base.OnThisPlay();
	}

}
