using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileMadness : Card
{
	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		for (int i = 0; i < 2; i++) for (int j = 0; j < 5; j++) if (Tile.GetTeamTiles(GetOpponent(team))[i, j].HasRevealedPlanted() && Tile.GetTeamTiles(GetOpponent(team))[i, j].planted.untrickable == 0)
				{
					if (row == i && col == j) StartCoroutine(Tile.GetTeamTiles(GetOpponent(team))[i, j].planted.ReceiveDamage(3, this));
					else StartCoroutine(Tile.GetTeamTiles(GetOpponent(team))[i, j].planted.ReceiveDamage(1, this));
                }
		yield return base.OnThisPlay();
	}

	public override bool IsValidTarget(BoxCollider2D bc)
	{
        if (!base.IsValidTarget(bc)) return false;
        Tile t = bc.GetComponent<Tile>();
		if (t == null) return false;
		if (t.HasRevealedPlanted() && t.planted.team != team) return true;
		return false;
	}

}
