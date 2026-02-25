using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whipvine : Card
{

	private Card toMove;

	protected override IEnumerator OnThisPlay()
	{
		for (int row = 0; row < Tile.ROWS; row++)
			for (int col = 0; col < Tile.COLUMNS; col++)
			{
                if (Tile.GetTeamTiles(GetOpponent(team))[row, col].HasRevealedPlanted()) choices.Add(Tile.GetTeamTiles(GetOpponent(team))[row, col].GetComponent<BoxCollider2D>());
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
        for (int r = 0; r < Tile.ROWS; r++) for (int col = 0; col < 5; col++) if (Tile.CanPlantInCol(col, Tile.GetTeamTiles(GetOpponent(team)), toMove.teamUp, toMove.amphibious))
                choices.Add(Tile.GetTeamTiles(GetOpponent(team))[r, col].GetComponent<BoxCollider2D>());

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
		yield return Glow();
		toMove.Move(t.row, t.col);
	}

}
