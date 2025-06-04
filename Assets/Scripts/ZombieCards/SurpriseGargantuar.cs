using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurpriseGargantuar : Card
{

	protected override IEnumerator OnThisPlay()
	{
		for (int col = 0; col < 4; col++)
		{
            if (Tile.zombieTiles[0, col].planted == null) choices.Add(Tile.zombieTiles[0, col].GetComponent<BoxCollider2D>());
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
		Tile t = bc.GetComponent<Tile>();
		yield return new WaitForSeconds(1);
		Move(t.row, t.col);
	}

}
