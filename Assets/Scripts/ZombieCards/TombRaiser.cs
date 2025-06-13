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
                if (team == GameManager.Instance.team)
                {
                    for (int n = choices.Count - 1; n > 0; n--)
                    {
                        int k = UnityEngine.Random.Range(0, n + 1);
                        var temp = choices[n];
                        choices[n] = choices[k];
                        choices[k] = temp;
                    }
                    GameManager.Instance.StoreRpc(choices[0].GetComponent<Tile>().row + " - " + choices[0].GetComponent<Tile>().col + " - " + RandomGravestone());
                }
                yield return new WaitUntil(() => GameManager.Instance.shuffledList != null);

                Tile t = Tile.zombieTiles[int.Parse(GameManager.Instance.shuffledList[0]), int.Parse(GameManager.Instance.shuffledList[1])];
                int card = int.Parse(GameManager.Instance.shuffledList[2]);
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
