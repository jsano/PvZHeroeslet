using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheGreatZucchini : Card
{

	protected override IEnumerator OnThisPlay()
	{
		GameManager.Instance.DisableHandCards();
		
        List<Card> toDestroy = new();
        for (int col = 0; col < 5; col++)
        {
            Tile t = Tile.zombieTiles[0, col];
            if (t.planted != null && !t.planted.gravestone)
            {
                toDestroy.Add(t.planted);
                t.planted = null;
            }
        }
        yield return new WaitForSeconds(1);
        List<Card> todo = new();
        foreach (Card c in toDestroy)
		{
            Tile t = Tile.zombieTiles[c.row, c.col];
            string[] options = new string[] { "Baseball", "Cardboard Robot", "Backup Dancer", "Imp", "Skunk Punk" };
            if (GameManager.Instance.team == team) GameManager.Instance.PlayCardRpc(HandCard.MakeDefaultFS(AllCards.NameToID(options[Random.Range(0, options.Length)])), t.row, t.col, true);
            else yield return new WaitUntil(() => t.planted != null);
            t.planted.playState = PlayState.Waiting;
            todo.Add(t.planted);
        }

        foreach (Card c in toDestroy) Destroy(c.gameObject);

        foreach (Card c in todo)
        {
            c.playState = PlayState.ReadyForOnThisPlay;
            yield return new WaitUntil(() => c.playState == PlayState.OnThisPlayed);
        }

        yield return base.OnThisPlay();
	}

}
