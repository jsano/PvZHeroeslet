using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombotAerostaticGondola : Card
{

	protected override IEnumerator OnCardHurt(Tuple<Damagable, Card, int, int> hurt)
	{
        choices.Clear();
        if (hurt.Item2 == this && hurt.Item1 == GameManager.Instance.plantHero)
        {
			if (GameManager.Instance.team == team)
			{
                for (int j = 0; j < 5; j++)
                {
                    if (j != col && Tile.zombieTiles[row, j].planted == null)
                    {
                        choices.Add(Tile.zombieTiles[row, j].GetComponent<BoxCollider2D>());
                    }
                }
                for (int n = choices.Count - 1; n > 0; n--)
                {
                    int k = UnityEngine.Random.Range(0, n + 1);
                    var temp = choices[n];
                    choices[n] = choices[k];
                    choices[k] = temp;
                }
                if (choices.Count > 0) GameManager.Instance.StoreRpc(choices[0].GetComponent<Tile>().row + " - " + choices[0].GetComponent<Tile>().col + " - " + AllCards.RandomFromCost(Team.Zombie, (0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12), true));
            }
			yield return new WaitForSeconds(1);
            if (GameManager.Instance.shuffledList != null) {
                Move(int.Parse(GameManager.Instance.shuffledList[0]), int.Parse(GameManager.Instance.shuffledList[1]));
                Card c = Instantiate(AllCards.Instance.cards[int.Parse(GameManager.Instance.shuffledList[2])]);
                Tile.zombieTiles[oldRow, oldCol].Plant(c);
            }
        }
		yield return base.OnCardHurt(hurt);
	}

}
