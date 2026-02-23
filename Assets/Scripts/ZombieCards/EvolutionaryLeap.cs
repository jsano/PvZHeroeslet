using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolutionaryLeap : Card
{

	protected override IEnumerator OnThisPlay()
	{
        Card c = Tile.GetTeamTiles(team)[row, col].planted;
        Tile.GetTeamTiles(team)[row, col].Unplant(true);
        yield return new WaitForSeconds(1);
        yield return SyncRandomChoiceAcrossNetwork(AllCards.RandomFromCost((c.cost + 1, c.cost + 1), true) + "");
        Card c1 = Instantiate(AllCards.Instance.cards[int.Parse(GameManager.Instance.GetShuffledList()[0])]);
        Tile.GetTeamTiles(team)[row, col].Plant(c1);
        Destroy(c.gameObject);
        yield return base.OnThisPlay();
	}

	public override bool IsValidTarget(BoxCollider2D bc)
	{
        if (!base.IsValidTarget(bc)) return false;
        Tile t = bc.GetComponent<Tile>();
		if (t == null) return false;
		if (t.HasRevealedPlanted() && t.planted.team == GameManager.Instance.team) return true;
		return false;
	}

}
