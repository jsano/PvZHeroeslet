using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Molekale : Card
{

	protected override IEnumerator OnThisPlay()
	{		
        List<Card> toDestroy = new();
        for (int col = 4; col >= 0; col--)
        {
            for (int row = 1; row >= 0; row--)
            {
                Tile t = Tile.plantTiles[row, col];
                if (t.HasRevealedPlanted() && t.planted != this)
                {
                    toDestroy.Add(t.planted);
                    t.Unplant(true);
                }
            }
        }
        if (toDestroy.Count > 0)
        {
            yield return new WaitForSeconds(1);
            string s = "";
            for (int i = 0; i < toDestroy.Count; i++)
            {
                s += AllCards.RandomFromCost(toDestroy[i].team, (toDestroy[i].cost + 1, toDestroy[i].cost + 1), true) + " - ";
            }
            yield return SyncRandomChoiceAcrossNetwork(s);
            for (int i = 0; i < GameManager.Instance.GetShuffledList().Count - 1; i++)
            {
                Card c = Instantiate(AllCards.Instance.cards[int.Parse(GameManager.Instance.GetShuffledList()[i])]);
                Tile.plantTiles[toDestroy[i].row, toDestroy[i].col].Plant(c);
            }
            foreach (Card c in toDestroy) Destroy(c.gameObject);
        }
        yield return base.OnThisPlay();
	}

}
