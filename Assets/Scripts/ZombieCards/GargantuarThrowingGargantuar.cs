using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GargantuarThrowingGargantuar : Card
{

	protected override IEnumerator OnCardHurt(Tuple<Damagable, Card, int, int> hurt)
    {
        if (hurt.Item1.GetComponent<Card>() == this)
        {
            yield return new WaitForSeconds(1);
            if (GameManager.Instance.team == team)
            {
                List<int> columns = new();
                for (int col = 0; col < 5; col++)
                {
                    if (Tile.zombieTiles[0, col].planted == null) columns.Add(col);
                }
                int id = AllCards.RandomFromTribe((Tribe.Gargantuar, Tribe.Gargantuar), true);
                GameManager.Instance.PlayCardRpc(new FinalStats(id), 0, columns[UnityEngine.Random.Range(0, columns.Count)]);
            }
        }

        yield return base.OnCardHurt(hurt);
	}

}
