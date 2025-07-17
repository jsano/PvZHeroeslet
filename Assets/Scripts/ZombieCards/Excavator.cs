using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Excavator : Card
{

	protected override IEnumerator OnThisPlay()
	{
		for (int col = 0; col < 5; col++)
		{
			for (int row = 0; row < 2; row++)
			{
				if (Tile.plantTiles[row, col].planted != null || Tile.terrainTiles[col].planted != null)
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
		Tile t = bc.GetComponent<Tile>();
		if (Tile.plantTiles[1, t.col].planted != null) Tile.plantTiles[1, t.col].planted.Bounce();
        if (Tile.plantTiles[0, t.col].planted != null) Tile.plantTiles[0, t.col].planted.Bounce();
        if (Tile.terrainTiles[t.col].planted != null) Tile.terrainTiles[t.col].planted.Bounce();
    }

}
