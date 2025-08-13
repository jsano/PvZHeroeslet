using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ImpThrowingImp : Card
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
                yield return SyncRandomChoiceAcrossNetwork(columns[UnityEngine.Random.Range(0, columns.Count)] + " - " + RandomImp());
                Card c = Instantiate(AllCards.Instance.cards[int.Parse(GameManager.Instance.GetShuffledList()[1])]).GetComponent<Card>();
                Tile.zombieTiles[0, int.Parse(GameManager.Instance.GetShuffledList()[0])].Plant(c);
            }
        }

        yield return base.OnCardHurt(hurt);
	}

    private int RandomImp()
    {
        List<int> possible = new();
        for (int i = 0; i < AllCards.Instance.cards.Length; i++)
        {
            if (AllCards.Instance.cards[i].tribes.Contains(Tribe.Imp) && AllCards.Instance.cards[i].cost <= 2 && AllCards.Instance.cards[i].type == Type.Unit)
            {
                possible.Add(i);
            }
        }
        return possible[UnityEngine.Random.Range(0, possible.Count)];
    }

}
