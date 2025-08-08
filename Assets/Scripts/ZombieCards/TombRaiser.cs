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
                yield return new WaitForSeconds(1);
                var choice = choices[UnityEngine.Random.Range(0, choices.Count)];
                yield return SyncRandomChoiceAcrossNetwork(choice.GetComponent<Tile>().row + " - " + choice.GetComponent<Tile>().col + " - " + RandomGravestone());
                Tile t = Tile.zombieTiles[int.Parse(GameManager.Instance.GetShuffledList()[0]), int.Parse(GameManager.Instance.GetShuffledList()[1])];
                int card = int.Parse(GameManager.Instance.GetShuffledList()[2]);
                if (GameManager.Instance.team == team) GameManager.Instance.PlayCardRpc(new FinalStats(card), t.row, t.col);
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
