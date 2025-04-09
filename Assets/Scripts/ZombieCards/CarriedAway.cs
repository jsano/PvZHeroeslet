using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarriedAway : Card
{

	protected override IEnumerator OnThisPlay()
	{
		if (GameManager.Instance.team == team)
		{
			for (int col = 0; col < 5; col++)
			{
				if (Tile.zombieTiles[0, col].planted == null && (col != 5 || Tile.zombieTiles[0, this.col].planted.amphibious))
				{
					choices.Add(Tile.zombieTiles[0, col].GetComponent<BoxCollider2D>());
				}
			}
			if (choices.Count == 1) StartCoroutine(OnSelection(choices[0]));
			if (choices.Count >= 2)
			{
				selected = false;
			}
		}
		GameManager.Instance.selecting = true;
		yield return base.OnThisPlay();
	}

	protected override IEnumerator OnSelection(BoxCollider2D bc)
	{
		yield return new WaitForSeconds(1);
		Tile t = bc.GetComponent<Tile>();
		GameManager.Instance.MoveRpc(team, row, col, t.row, t.col);
		yield return new WaitUntil(() => t.planted != null);
        GameManager.Instance.BonusAttackRpc(team, t.row, t.col);
		yield return new WaitForSeconds(1); // TODO: fix??
        GameManager.Instance.EndSelectingRpc();
    }

	public override bool IsValidTarget(BoxCollider2D bc)
	{
		Tile t = bc.GetComponent<Tile>();
		if (t == null) return false;
		if (t.HasRevealedPlanted()) return true;
		return false;
	}

}
