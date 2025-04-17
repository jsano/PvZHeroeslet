using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweetPotato : Card
{
    private bool canMove;

	protected override IEnumerator OnThisPlay()
	{
        canMove = true;
        for (int col = 0; col < 5; col++)
        {
            if (Tile.zombieTiles[0, col].planted != null && col == this.col) 
            {
                canMove = false;
                break;
            }
            if (Tile.zombieTiles[0, col].HasRevealedPlanted()) 
                choices.Add(Tile.zombieTiles[0, col].planted.GetComponent<BoxCollider2D>());
            Debug.Log(Tile.zombieTiles[0, col].planted);
        }
		if (GameManager.Instance.team == team)
		{
            if (!canMove) 
            {
                yield return base.OnThisPlay();
                yield break;
            }
			if (choices.Count == 1) StartCoroutine(OnSelection(choices[0]));
			if (choices.Count >= 2)
            {
                selected = false;
            }
		}
        if (choices.Count > 0) GameManager.Instance.selecting = true;
		Debug.Log(selected);
		yield return base.OnThisPlay();
	}

	protected override IEnumerator OnSelection(BoxCollider2D bc)
	{
        yield return new WaitForSeconds(1);
		Card c = bc.GetComponent<Card>();
        Debug.Log(c != null ? c : "null");
        Debug.Log(c + " " + c.row + " " + c.col);
        GameManager.Instance.MoveRpc(c.team, c.row, c.col, c.row, col);
        GameManager.Instance.EndSelectingRpc();
    }

}
