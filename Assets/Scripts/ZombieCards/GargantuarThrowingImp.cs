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
            yield return new WaitForSeconds(1);
            List<int> columns = new();
            for (int col = 0; col < 5; col++)
            {
                if (Tile.zombieTiles[0, col].planted == null) columns.Add(col);
            }
            if (columns.Count > 0)
            {
                yield return new WaitForSeconds(1);
                yield return SyncRandomChoiceAcrossNetwork(columns[UnityEngine.Random.Range(0, columns.Count)] + " - " + Random5Gargantuar());
                Card c = Instantiate(AllCards.Instance.cards[int.Parse(GameManager.Instance.shuffledLists[^1][1])]).GetComponent<Card>();
                Tile.zombieTiles[0, int.Parse(GameManager.Instance.shuffledLists[^1][0])].Plant(c);
            }
        }

        yield return base.OnCardHurt(hurt);
	}

    public static int Random5Gargantuar()
    {
        List<int> possible = new();
        for (int i = 0; i < AllCards.Instance.cards.Length; i++)
        {
            if (AllCards.Instance.cards[i].tribes.Contains(Tribe.Gargantuar) && AllCards.Instance.cards[i].cost == 5)
            {
                possible.Add(i);
            }
        }
        return possible[UnityEngine.Random.Range(0, possible.Count)];
    }

}
