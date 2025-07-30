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
		if (GameManager.Instance.team == team) GameManager.Instance.StoreRpc(AllCards.RandomFromCost(Team.Zombie, (1, 1), true) + "");
		yield return new WaitUntil(() => GameManager.Instance.shuffledList != null);
		Card c = Instantiate(AllCards.Instance.cards[int.Parse(GameManager.Instance.shuffledList[0])]);
		Tile.zombieTiles[row, col].Plant(c);
		Destroy(toDestroy.gameObject);
		yield return base.OnThisPlay();
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