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
				if (Tile.plantTiles[row, col].HasRevealedPlanted()) choices.Add(Tile.plantTiles[0, col].GetComponent<BoxCollider2D>());
                if (Tile.zombieTiles[row, col].HasRevealedPlanted()) choices.Add(Tile.zombieTiles[0, col].GetComponent<BoxCollider2D>());
            }
		if (GameManager.Instance.team == team)
		{
			if (choices.Count == 1) StartCoroutine(OnSelection(choices[0]));
			if (choices.Count >= 2)
			{
				selected = false;
			}
		}
        if (choices.Count > 0) GameManager.Instance.selecting = true;
		yield return base.OnThisPlay();
	}

	protected override IEnumerator OnSelection(BoxCollider2D bc)
	{
		if (toMove == null)
		{
			choices.Clear();
			Tile t = bc.GetComponent<Tile>();
			toMove = t.planted;
			var targets = toMove.team == Team.Plant ? Tile.plantTiles : Tile.zombieTiles;
			for (int row = 0; row < 2; row++) for (int col = 0; col < 5; col++)
				if (Tile.CanPlantInCol(col, targets, toMove.teamUp, toMove.amphibious))
					if (row == 0 || row == 1 && (toMove.teamUp || targets[0, col].planted != null && targets[0, col].planted.teamUp))
						choices.Add(targets[row, col].GetComponent<BoxCollider2D>());

			if (choices.Count == 1) StartCoroutine(OnSelection(choices[0]));
			if (choices.Count >= 2)
			{
				selected = false;
			}
		}
		else
		{
            Tile t = bc.GetComponent<Tile>();
            yield return new WaitForSeconds(1);
			GameManager.Instance.MoveRpc(toMove.team, toMove.row, toMove.col, t.row, t.col);
			GameManager.Instance.EndSelectingRpc();
		}
    }

}
