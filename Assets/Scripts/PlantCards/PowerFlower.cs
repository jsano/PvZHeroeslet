using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerFlower : Card
{

	protected override IEnumerator OnTurnStart()
	{
        yield return Glow();
        int count = 0;
        for (int row = 0; row < Tile.ROWS; row++)
        {
            for (int col = 0; col < Tile.COLUMNS; col++)
            {
                Card c = Tile.GetTeamTiles(team)[row, col].planted;
                if (c != null && c.tribes.Contains(Tribe.Flower)) count++;
            }
        }
        yield return GameManager.Instance.GetTeamHero(team).Heal(count);
        yield return base.OnTurnStart();
	}

}
