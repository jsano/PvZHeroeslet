using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarriedAway : Card
{

	protected override IEnumerator OnThisPlay()
	{
		for (int col = 0; col < 5; col++)
		{
			if (Tile.zombieTiles[0, col].planted == null && (col != 5 || Tile.zombieTiles[0, this.col].planted.amphibious))
			{
				choices.Add(Tile.zombieTiles[0, col].GetComponent<BoxCollider2D>());
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
        Tile.zombieTiles[row, col].planted.Move(t.row, t.col);
		yield return new WaitForSeconds(0.5f);
        yield return Tile.zombieTiles[t.row, t.col].planted.BonusAttack();
    }

	public override bool IsValidTarget(BoxCollider2D bc)
	{
        if (!base.IsValidTarget(bc)) return false;
        Tile t = bc.GetComponent<Tile>();
		if (t == null) return false;
		if (t.HasRevealedPlanted() && t.planted.team == Team.Zombie) return true;
		return false;
	}

}
