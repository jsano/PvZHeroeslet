using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombotDinotronicMechasaur : Card
{

	protected override IEnumerator OnCardDraw(Team team)
    {
        if (team == this.team)
        {
            yield return new WaitForSeconds(1);
            if (GameManager.Instance.team == team)
            {
                List<int> columns = new();
                for (int col = 0; col < 5; col++)
                {
                    if (Tile.zombieTiles[0, col].planted == null) columns.Add(col);
                }
                if (columns.Count > 0)
                {
                    int id = AllCards.RandomFromTribe((Tribe.History, Tribe.History), true);
                    GameManager.Instance.PlayCardRpc(new FinalStats(id), 0, columns[UnityEngine.Random.Range(0, columns.Count)]);
                }
            }
        }
        yield return base.OnCardDraw(team);
	}

}
