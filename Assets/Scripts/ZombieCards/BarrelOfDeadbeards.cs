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
            
			for (int i = 0; i < 2; i++) for (int j = 0; j < 5; j++)
				{
					if (Tile.plantTiles[i, j].planted != null) targets.Add(Tile.plantTiles[i, j].planted);
                    if (Tile.zombieTiles[i, j].planted != null && Tile.zombieTiles[i, j].planted != this) targets.Add(Tile.zombieTiles[i, j].planted);
                }
            yield return Glow();
            yield return AttackFXs(targets);
			foreach (Damagable d in targets) StartCoroutine(d.ReceiveDamage(1, this));
            Card card = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Captain Deadbeard")]).GetComponent<Card>();
			Tile.zombieTiles[row, col].Unplant();
            Tile.zombieTiles[row, col].Plant(card);
        }
		yield return base.OnCardDeath(died);
	}

}
