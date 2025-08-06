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
            for (int j = 0; j < 5; j++)
            {
                if (j != col && Tile.zombieTiles[row, j].planted == null)
                {
                    choices.Add(Tile.zombieTiles[row, j].GetComponent<BoxCollider2D>());
                }
            }
            if (choices.Count > 0)
            {
                yield return new WaitForSeconds(1);
                var choice = choices[UnityEngine.Random.Range(0, choices.Count)];
                yield return SyncRandomChoiceAcrossNetwork(choice.GetComponent<Tile>().row + " - " + choice.GetComponent<Tile>().col + " - " + AllCards.RandomFromCost(Team.Zombie, (0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12), true));
                Move(int.Parse(GameManager.Instance.shuffledLists[^1][0]), int.Parse(GameManager.Instance.shuffledLists[^1][1]));
                Card c = Instantiate(AllCards.Instance.cards[int.Parse(GameManager.Instance.shuffledLists[^1][2])]);
                Tile.zombieTiles[oldRow, oldCol].Plant(c);
            }
        }
		yield return base.OnCardHurt(hurt);
	}

}
