using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summoning : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
        yield return SyncRandomChoiceAcrossNetwork(AllCards.RandomFromCost(Team.B, (0, 1, 2), true, col == 4) + "");
        Card c = Instantiate(AllCards.Instance.cards[int.Parse(GameManager.Instance.GetShuffledList()[0])]);
        Tile.GetTeamTiles(team)[row, col].Plant(c);
        yield return base.OnThisPlay();
	}

	public override bool IsValidTarget(BoxCollider2D bc)
	{
		Tile t = bc.GetComponent<Tile>();
		if (t == null) return false;
		if (!t.isPlayerTile) return false;
		if (t.planted == null) return true;
		return false;
	}

}