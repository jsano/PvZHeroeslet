using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeBomb : Card
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
		GameManager.Instance.RaiseAttackRpc(team, row, col, 1);
		GameManager.Instance.MoveRpc(team, row, col, t.row, t.col);
		GameManager.Instance.EndSelectingRpc();
    }

	public override bool IsValidTarget(BoxCollider2D bc)
	{
		Tile t = bc.GetComponent<Tile>();
		if (t == null) return false;
        if (t.HasRevealedPlanted() && t.planted.team == Team.Zombie) return true;
        return false;
    }

}
