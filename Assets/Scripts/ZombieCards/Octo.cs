using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Octo : Card
{

	protected override IEnumerator OnThisPlay()
	{
		for (int col = 0; col < 5; col++)
		{
			if (Tile.zombieTiles[0, col].planted == null)
			{
				choices.Add(Tile.zombieTiles[0, col].GetComponent<BoxCollider2D>());
			}
		}
        if (choices.Count == 1) yield return OnSelection(choices[0]);
        if (choices.Count >= 2)
        {
            if (GameManager.Instance.team == team) selected = false;
            yield return new WaitUntil(() => GameManager.Instance.selection != null);
            yield return OnSelection(GameManager.Instance.selection);
        }
        yield return base.OnThisPlay();
	}

	protected override IEnumerator OnSelection(BoxCollider2D bc)
	{
        yield return base.OnSelection(bc);
        yield return Glow();
        Tile t = bc.GetComponent<Tile>();
        Card card = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Octo-pet")]);
        Tile.zombieTiles[t.row, t.col].Plant(card);
    }

	protected override IEnumerator OnCardDeath(Tuple<Card, Card> died)
	{
		if (died.Item1 == this)
		{
            yield return Glow();
            yield return GameManager.Instance.GainHandCard(team, AllCards.NameToID("Octo Zombie"));
		}
		yield return base.OnCardDeath(died);
	}

}
