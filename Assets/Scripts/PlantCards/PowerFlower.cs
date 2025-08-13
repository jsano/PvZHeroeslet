using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerFlower : Card
{

	protected override IEnumerator OnTurnStart()
	{
        yield return Glow();
        int count = 0;
        for (int row = 0; row < 2; row++)
        {
            for (int col = 0; col < 5; col++)
            {
                Card c = Tile.plantTiles[row, col].planted;
                if (c != null && c.tribes.Contains(Tribe.Flower)) count++;
            }
        }
        yield return GameManager.Instance.plantHero.Heal(count);
        yield return base.OnTurnStart();
	}

}
