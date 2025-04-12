using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summoning : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		if (GameManager.Instance.team == team) GameManager.Instance.PlayCardRpc(FinalStats.MakeDefaultFS(AllCards.RandomFromCost(Team.Zombie, (0, 1, 2), true, col == 4)), 0, col, true);
		yield return base.OnThisPlay();
	}

	public override bool IsValidTarget(BoxCollider2D bc)
	{
		Tile t = bc.GetComponent<Tile>();
		if (t == null) return false;
		if (t.isPlantTile) return false;
		if (t.planted == null) return true;
		return false;
	}

}