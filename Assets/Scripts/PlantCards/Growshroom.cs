using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Growshroom : Card
{

	protected override IEnumerator OnThisPlay()
	{
		for (int row = 0; row < 2; row++)
		{
			for (int col = 0; col < 5; col++)
			{
				if (Tile.plantTiles[row, col].planted != null && Tile.plantTiles[row, col].planted != this)
				{
					choices.Add(Tile.plantTiles[row, col].GetComponent<BoxCollider2D>());
				}
			}
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
		c.ChangeStats(2, 2);
    }

}
