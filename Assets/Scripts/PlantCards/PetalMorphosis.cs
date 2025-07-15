using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetalMorphosis : Card
{

	protected override IEnumerator OnThisPlay()
	{
		Card toDestroy = Tile.plantTiles[row, col].planted;
		Tile.plantTiles[row, col].Unplant(true);
		yield return new WaitForSeconds(1);
		Destroy(toDestroy.gameObject);
		if (GameManager.Instance.team == team) GameManager.Instance.PlayCardRpc(new FinalStats(AllCards.RandomFromCost(Team.Plant, (0,1,2,3,4,5,6,7,8,9,10), true, col == 4)), row, col);
		yield return GameManager.Instance.DrawCard(team);
		yield return base.OnThisPlay();
	}

	public override bool IsValidTarget(BoxCollider2D bc)
	{
        if (!base.IsValidTarget(bc)) return false;
        Tile t = bc.GetComponent<Tile>();
		if (t == null) return false;
		if (t.HasRevealedPlanted() && t.planted.team == Team.Plant) return true;
		return false;
	}

}