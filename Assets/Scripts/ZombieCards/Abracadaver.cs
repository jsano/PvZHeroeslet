using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abracadaver : Card
{

	protected override IEnumerator OnCardHurt(Tuple<Damagable, Card, int, int> hurt)
	{
        choices.Clear();
		if (hurt.Item2 == this && hurt.Item1.GetComponent<Hero>() != null) 
		{
            for (int i = 0; i < 2; i++) for (int j = 0; j < 5; j++)
            {
                if (Tile.plantTiles[i, j].planted != null) choices.Add(Tile.plantTiles[i, j].GetComponent<BoxCollider2D>());
            }
            if (choices.Count > 0)
            {
                var choice = choices[UnityEngine.Random.Range(0, choices.Count)];
                yield return SyncRandomChoiceAcrossNetwork(choice.GetComponent<Tile>().row + " - " + choice.GetComponent<Tile>().col);
                Tile t = Tile.plantTiles[int.Parse(GameManager.Instance.GetShuffledList()[0]), int.Parse(GameManager.Instance.GetShuffledList()[1])];
                yield return Glow();
                yield return AttackFX(t.planted);
                yield return t.planted.ReceiveDamage(3, this);
            }
        }
		yield return base.OnCardHurt(hurt);
	}

}
