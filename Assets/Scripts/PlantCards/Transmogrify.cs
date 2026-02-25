using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transmogrify : Card
{

	protected override IEnumerator OnThisPlay()
	{
		Card toDestroy = Tile.GetTeamTiles(GetOpponent(team))[row, col].planted;
		Tile.GetTeamTiles(GetOpponent(team))[row, col].Unplant(true);
		yield return new WaitForSeconds(1);
        yield return SyncRandomChoiceAcrossNetwork(AllCards.RandomFromCost((1, 1), true) + "");
		Card c = Instantiate(AllCards.Instance.cards[int.Parse(GameManager.Instance.GetShuffledList()[0])]);
		Tile.GetTeamTiles(GetOpponent(team))[row, col].Plant(c);
		Destroy(toDestroy.gameObject);
		yield return base.OnThisPlay();
	}

	public override bool IsValidTarget(BoxCollider2D bc)
	{
        if (!base.IsValidTarget(bc)) return false;
        Tile t = bc.GetComponent<Tile>();
		if (t == null) return false;
		if (t.HasRevealedPlanted() && t.planted.team == GetOpponent(GameManager.Instance.team)) return true;
		return false;
	}

}