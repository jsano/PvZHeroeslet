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
            foreach (Card c in toDestroy)
		    {
                Tile t = Tile.plantTiles[c.row, c.col];
                int ID = AllCards.RandomFromCost(c.team, (c.cost + 1, c.cost + 1), true);
                if (GameManager.Instance.team == team) GameManager.Instance.PlayCardRpc(new FinalStats(ID), t.row, t.col);
            }

            foreach (Card c in toDestroy) Destroy(c.gameObject);
        }
        

        yield return base.OnThisPlay();
	}

}
