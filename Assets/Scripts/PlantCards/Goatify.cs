using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goatify : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		Destroy(Tile.zombieTiles[row, col].planted.gameObject);
        Tile.zombieTiles[row, col].Unplant();
        Card card = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Goat")]).GetComponent<Card>();
        Tile.zombieTiles[row, col].Plant(card);
        yield return base.OnThisPlay();
	}

	public override bool IsValidTarget(BoxCollider2D bc)
	{
        if (!base.IsValidTarget(bc)) return false;
        List<BoxCollider2D> targets = new();
        int highest = -1;
        for (int i = 0; i < 5; i++) if (Tile.zombieTiles[0, i].HasRevealedPlanted() && Tile.zombieTiles[0, i].planted.atk > highest)
            {
                highest = Tile.zombieTiles[0, i].planted.atk;
            }
        for (int i = 0; i < 5; i++) if (Tile.zombieTiles[0, i].HasRevealedPlanted() && Tile.zombieTiles[0, i].planted.atk == highest)
            {
                targets.Add(Tile.zombieTiles[0, i].GetComponent<BoxCollider2D>());
            }
        if (targets.Contains(bc)) return true;
        return false;
    }

}