using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweetPotato : Card
{

	protected override IEnumerator OnThisPlay()
	{
        for (int col = 0; col < 5; col++)
        {
            if (Tile.zombieTiles[0, col].planted != null && col == this.col) 
            {
                yield return base.OnThisPlay();
                yield break;
            }
            if (Tile.zombieTiles[0, col].HasRevealedPlanted()) choices.Add(Tile.zombieTiles[0, col].GetComponent<BoxCollider2D>());
        }
        if (choices.Count == 1) yield return OnSelection(choices[0]);
        if (choices.Count >= 2)
        {
            if (GameManager.Instance.team == team) selected = false;
            yield return new WaitUntil(() => GameManager.Instance.selection != null);
            yield return OnSelection(GameManager.Instance.selection);
        }
        yield return base.OnThisPlay();
	}

	protected override IEnumerator OnSelection(BoxCollider2D bc)
	{
        yield return base.OnSelection(bc);
        yield return new WaitForSeconds(1);
		Card c = bc.GetComponent<Tile>().planted;
        c.Move(c.row, col);
    }

}
