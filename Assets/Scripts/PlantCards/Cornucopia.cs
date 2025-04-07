using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cornucopia : Card
{

	protected override IEnumerator OnThisPlay()
	{		
        yield return new WaitForSeconds(1);
        if (GameManager.Instance.team == team)
        {
            for (int col = 0; col < 5; col++)
		    {
                if (!Tile.CanPlantInCol(col, Tile.plantTiles, false, true)) continue;
                GameManager.Instance.PlayCardRpc(FinalStats.MakeDefaultFS(AllCards.RandomFromCost(team, (0,1,2,3,4,5,6,7,8,9,10,11,12), true, col == 4)), 0, col, true);
            }
        }

        yield return base.OnThisPlay();
	}

}
