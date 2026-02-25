using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWithAttack : Card
{

	public int attackCutoff;
	public enum Compare
	{
		OrLess,
		OrMore
	}
	public Compare comparator;

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		Tile.GetTeamTiles(GetOpponent(team))[row, col].planted.Destroy();
		yield return base.OnThisPlay();
	}

	public override bool IsValidTarget(BoxCollider2D bc)
	{
        if (!base.IsValidTarget(bc)) return false;
        Tile t = bc.GetComponent<Tile>();
		if (t == null) return false;
		if (t.HasRevealedPlanted() && t.planted.team == GetOpponent(GameManager.Instance.team))
		{
			if (comparator == Compare.OrLess && t.planted.atk <= attackCutoff) return true;
            if (comparator == Compare.OrMore && t.planted.atk >= attackCutoff) return true;
        } 
		return false;
	}

}
