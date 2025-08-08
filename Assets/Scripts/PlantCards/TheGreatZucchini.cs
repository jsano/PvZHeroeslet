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
                t.Unplant(true);
            }
        }
        if (toDestroy.Count > 0)
        {
            yield return new WaitForSeconds(1);
            string[] options = new string[] { "Baseball Zombie", "Cardboard Robot", "Backup Dancer", "Imp", "Skunk Punk" };
            string s = "";
            for (int i = 0; i < toDestroy.Count; i++)
            {
                s += options[UnityEngine.Random.Range(0, options.Length)] + " - ";
            }
            yield return SyncRandomChoiceAcrossNetwork(s);
            for (int i = 0; i < GameManager.Instance.GetShuffledList().Count - 1; i++)
            {
                Card c = Instantiate(AllCards.Instance.cards[AllCards.NameToID(GameManager.Instance.GetShuffledList()[i])]);
                Tile.zombieTiles[0, toDestroy[i].col].Plant(c);
            }
            foreach (Card c in toDestroy) Destroy(c.gameObject);
        }
        

        yield return base.OnThisPlay();
	}

}
