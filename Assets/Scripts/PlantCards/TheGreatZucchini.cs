using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheGreatZucchini : Card
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
                t.planted = null;
            }
        }
        if (toDestroy.Count > 0)
        {
            yield return new WaitForSeconds(1);
            foreach (Card c in toDestroy)
		    {
                Tile t = Tile.zombieTiles[c.row, c.col];
                string[] options = new string[] { "Baseball", "Cardboard Robot", "Backup Dancer", "Imp", "Skunk Punk" };
                if (GameManager.Instance.team == team) GameManager.Instance.PlayCardRpc(FinalStats.MakeDefaultFS(AllCards.NameToID(options[Random.Range(0, options.Length)])), t.row, t.col, true);
            }

            foreach (Card c in toDestroy) Destroy(c.gameObject);
        }
        

        yield return base.OnThisPlay();
	}

}
