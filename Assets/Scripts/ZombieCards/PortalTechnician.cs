using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PortalTechnician : Card
{

	protected override IEnumerator OnCardDeath(Card died)
	{
		if (died == this)
		{
            Tile.zombieTiles[row, col].Unplant(true);
            yield return new WaitForSeconds(1);
            if (GameManager.Instance.team == team) GameManager.Instance.PlayCardRpc(new FinalStats(AllCards.RandomFromCost(team, (4, 5, 6, 7, 8, 9, 10, 11), true)), row, col);
        }
		yield return base.OnCardDeath(died);
	}

}
