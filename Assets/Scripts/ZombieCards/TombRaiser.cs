using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TombRaiser : Card
{

	protected override IEnumerator OnCardHurt(Tuple<Damagable, Card, int, int> hurt)
	{
        choices.Clear();
		if (hurt.Item2 == this && hurt.Item1.GetComponent<Hero>() != null) 
		{
            for (int j = 0; j < 4; j++)
            {
                if (Tile.zombieTiles[0, j].planted == null) choices.Add(Tile.zombieTiles[0, j].GetComponent<BoxCollider2D>());
            }
            if (choices.Count > 0)
            {
                yield return Glow();
                var choice = choices[UnityEngine.Random.Range(0, choices.Count)];
                yield return SyncRandomChoiceAcrossNetwork(choice.GetComponent<Tile>().row + " - " + choice.GetComponent<Tile>().col + " - " + RandomGravestone());
                Tile t = Tile.zombieTiles[int.Parse(GameManager.Instance.GetShuffledList()[0]), int.Parse(GameManager.Instance.GetShuffledList()[1])];
                Card c = Instantiate(AllCards.Instance.cards[int.Parse(GameManager.Instance.GetShuffledList()[2])]);
                Tile.zombieTiles[t.row, t.col].Plant(c);
            }
        }
		yield return base.OnCardHurt(hurt);
	}

    private int RandomGravestone()
    {
        List<int> possible = new();
        for (int i = 0; i < AllCards.Instance.cards.Length; i++)
        {
            if (AllCards.Instance.cards[i].team == team && AllCards.Instance.cards[i].gravestone)
            {
                possible.Add(i);
            }
        }
        return possible[UnityEngine.Random.Range(0, possible.Count)];
    }

}
