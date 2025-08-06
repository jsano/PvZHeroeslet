using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolutionaryLeap : Card
{

	protected override IEnumerator OnThisPlay()
	{
        Card c = Tile.zombieTiles[row, col].planted;
        Tile.zombieTiles[row, col].Unplant(true);
        yield return new WaitForSeconds(1);
        yield return SyncRandomChoiceAcrossNetwork(AllCards.RandomFromCost(Team.Zombie, (c.cost + 1, c.cost + 1), true) + "");
        Card c1 = Instantiate(AllCards.Instance.cards[int.Parse(GameManager.Instance.shuffledLists[^1][0])]);
        Tile.zombieTiles[row, col].Plant(c1);
        Destroy(c.gameObject);
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
