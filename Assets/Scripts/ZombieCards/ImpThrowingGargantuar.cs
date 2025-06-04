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
            if (GameManager.Instance.team == team)
            {
                List<int> columns = new();
                for (int col = 0; col < 5; col++)
                {
                    if (Tile.zombieTiles[0, col].planted == null) columns.Add(col);
                }
                GameManager.Instance.PlayCardRpc(new FinalStats(AllCards.NameToID("Swabbie")), 0, columns[UnityEngine.Random.Range(0, columns.Count)], true);
            }
        }

        yield return base.OnCardHurt(hurt);
	}

}
