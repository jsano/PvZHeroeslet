using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abracadaver : Card
{

	protected override IEnumerator OnCardHurt(Tuple<Damagable, Card, int, int> hurt)
	{
		if (hurt.Item2 == this && hurt.Item1.GetComponent<Hero>() != null) 
		{
            if (team == GameManager.Instance.team)
            {
                for (int i = 0; i < 2; i++) for (int j = 0; j < 5; j++)
                {
                    if (Tile.plantTiles[i, j].planted != null) choices.Add(Tile.plantTiles[i, j].GetComponent<BoxCollider2D>());
                }
                for (int n = choices.Count - 1; n > 0; n--)
                {
                    int k = UnityEngine.Random.Range(0, n + 1);
                    var temp = choices[n];
                    choices[n] = choices[k];
                    choices[k] = temp;
                }
                GameManager.Instance.StoreRpc(choices[0].GetComponent<Tile>().row + " - " + choices[0].GetComponent<Tile>().col);
            }
            yield return new WaitUntil(() => GameManager.Instance.shuffledList != null);

            Tile t = Tile.plantTiles[int.Parse(GameManager.Instance.shuffledList[0]), int.Parse(GameManager.Instance.shuffledList[1])];
            yield return AttackFX(t.planted);
            yield return t.planted.ReceiveDamage(3, this);
        }
		yield return base.OnCardHurt(hurt);
	}

}
