using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelOfDeadbeards : Card
{

	protected override IEnumerator OnCardDeath(Tuple<Card, Card> died)
	{
		if (died.Item1 == this)
		{
            List<Damagable> targets = new();
            
			for (int i = 0; i < Tile.ROWS; i++) for (int j = 0; j < Tile.COLUMNS; j++)
				{
					if (Tile.playerTiles[i, j].planted != null && Tile.playerTiles[i, j].planted != this) targets.Add(Tile.playerTiles[i, j].planted);
                    if (Tile.opponentTiles[i, j].planted != null && Tile.opponentTiles[i, j].planted != this) targets.Add(Tile.opponentTiles[i, j].planted);
                }
            yield return Glow();
            yield return AttackFXs(targets);
			foreach (Damagable d in targets) StartCoroutine(d.ReceiveDamage(1, this));
            Card card = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Captain Deadbeard")]).GetComponent<Card>();
			Tile.GetTeamTiles(team)[row, col].Unplant();
            Tile.GetTeamTiles(team)[row, col].Plant(card);
        }
		yield return base.OnCardDeath(died);
	}

}
