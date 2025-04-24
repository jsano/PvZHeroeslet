using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnsignUproot : Card
{

	private Card toMove;

	protected override IEnumerator OnThisPlay()
	{
		for (int row = 0; row < 2; row++)
			for (int col = 0; col < 5; col++)
			{
				if (Tile.plantTiles[row, col].HasRevealedPlanted() && Tile.plantTiles[row, col].planted != this) choices.Add(Tile.plantTiles[row, col].GetComponent<BoxCollider2D>());
                if (Tile.zombieTiles[row, col].HasRevealedPlanted()) choices.Add(Tile.zombieTiles[0, col].GetComponent<BoxCollider2D>());
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
        choices.Clear();
		Tile t = bc.GetComponent<Tile>();
		toMove = t.planted;
		var targets = toMove.team == Team.Plant ? Tile.plantTiles : Tile.zombieTiles;
		for (int row = 0; row < 2; row++) for (int col = 0; col < 5; col++)
			if (Tile.CanPlantInCol(col, targets, toMove.teamUp, toMove.amphibious))
				if (row == 0 || row == 1 && (toMove.teamUp || targets[0, col].planted != null && targets[0, col].planted.teamUp))
					choices.Add(targets[row, col].GetComponent<BoxCollider2D>());

        if (choices.Count == 1) yield return OnSelection1(choices[0]);
        if (choices.Count >= 2)
        {
            if (GameManager.Instance.team == team) selected = false;
            yield return new WaitUntil(() => GameManager.Instance.selection != null);
            yield return OnSelection1(GameManager.Instance.selection);
        }
    }

	private IEnumerator OnSelection1(BoxCollider2D bc)
	{
		GameManager.Instance.ClearSelection();
        choices.Clear();
        Tile t = bc.GetComponent<Tile>();
		yield return new WaitForSeconds(1);
		toMove.Move(t.row, t.col);
	}

}
