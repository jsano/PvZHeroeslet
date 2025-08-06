using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpThrowingGargantuar : Card
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
                yield return SyncRandomChoiceAcrossNetwork(columns[UnityEngine.Random.Range(0, columns.Count)] + "");
                Card c = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Swabbie")]).GetComponent<Card>();
                Tile.zombieTiles[0, int.Parse(GameManager.Instance.shuffledLists[^1][0])].Plant(c);
            }            
        }
        yield return base.OnCardHurt(hurt);
	}

}
