using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GargantuarThrowingImp : Card
{

	protected override IEnumerator OnCardHurt(Tuple<Damagable, Card, int, int> hurt)
    {
        if (hurt.Item1.GetComponent<Card>() == this)
        {
            List<int> columns = new();
            for (int col = 0; col < 5; col++)
            {
                if (Tile.zombieTiles[0, col].planted == null) columns.Add(col);
            }
            if (columns.Count > 0)
            {
                yield return Glow();
                yield return SyncRandomChoiceAcrossNetwork(columns[UnityEngine.Random.Range(0, columns.Count)] + " - " + AllCards.RandomTribeOfCost(Tribe.Gargantuar, 5, true));
                Card c = Instantiate(AllCards.Instance.cards[int.Parse(GameManager.Instance.GetShuffledList()[1])]).GetComponent<Card>();
                Tile.zombieTiles[0, int.Parse(GameManager.Instance.GetShuffledList()[0])].Plant(c);
            }
        }

        yield return base.OnCardHurt(hurt);
	}

}
