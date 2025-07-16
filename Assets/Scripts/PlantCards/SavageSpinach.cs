using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavageSpinach : Card
{

	protected override IEnumerator OnThisPlay()
	{
		if (evolved)
		{
			yield return new WaitForSeconds(1);
            for (int row = 0; row < 2; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    Card c = Tile.plantTiles[row, col].planted;
                    if (c != null) c.ChangeStats(2, 0);
                }
            }
            if (GameManager.Instance.team == team) foreach (HandCard hc in GameManager.Instance.GetHandCards()) if (hc.orig.type == Type.Unit) hc.ChangeAttack(2);
        }
		yield return base.OnThisPlay();
	}

}
