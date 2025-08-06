using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadMoonRising : Card
{

	protected override IEnumerator OnThisPlay()
	{		
        List<Card> toDestroy = new();
        for (int col = 4; col >= 0; col--)
        {
            Tile t = Tile.zombieTiles[0, col];
            if (t.HasRevealedPlanted())
            {
                toDestroy.Add(t.planted);
                t.Unplant(true);
            }
        }
        if (toDestroy.Count > 0)
        {
            yield return new WaitForSeconds(1);
            string s = AllCards.RandomFromCost(team, (5, 6, 7, 8, 9, 10, 11, 12), true) + "";
            for (int i = 1; i < toDestroy.Count; i++) s += " - " + AllCards.RandomFromCost(team, (5, 6, 7, 8, 9, 10, 11, 12), true);
            yield return SyncRandomChoiceAcrossNetwork(s);
            for (int i = 0; i < toDestroy.Count; i++)
            {
                Card c = Instantiate(AllCards.Instance.cards[int.Parse(GameManager.Instance.shuffledLists[^1][i])]);
                Tile.zombieTiles[0, toDestroy[i].col].Plant(c);
            }
            foreach (Card c in toDestroy) Destroy(c.gameObject);
        }
        yield return base.OnThisPlay();
	}

}
