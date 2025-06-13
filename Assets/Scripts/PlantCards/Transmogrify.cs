using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transmogrify : Card
{

	protected override IEnumerator OnThisPlay()
	{
		Card toDestroy = Tile.zombieTiles[row, col].planted;
		Tile.zombieTiles[row, col].Unplant(true);
		yield return new WaitForSeconds(1);
		Destroy(toDestroy.gameObject);
		if (GameManager.Instance.team == team) GameManager.Instance.PlayCardRpc(new FinalStats(AllCards.RandomFromCost(Team.Zombie, (1, 1), true, col == 4)), row, col);
		yield return base.OnThisPlay();
	}

	public override bool IsValidTarget(BoxCollider2D bc)
	{
		Tile t = bc.GetComponent<Tile>();
		if (t == null) return false;
		if (t.HasRevealedPlanted() && t.planted.team == Team.Zombie) return true;
		return false;
	}

}