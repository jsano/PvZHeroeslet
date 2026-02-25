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
            List<int> columns = new();
            for (int col = 0; col < 5; col++)
            {
                if (Tile.CanPlantInCol(col, Tile.GetTeamTiles(team), false, true)) columns.Add(col);
            }
            if (columns.Count > 0)
            {
                yield return Glow();
                yield return SyncRandomChoiceAcrossNetwork(columns[UnityEngine.Random.Range(0, columns.Count)] + "");
                Card c = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Swabbie")]).GetComponent<Card>();
                Tile.GetTeamTiles(team)[0, int.Parse(GameManager.Instance.GetShuffledList()[0])].Plant(c);
            }            
        }
        yield return base.OnCardHurt(hurt);
	}

}
