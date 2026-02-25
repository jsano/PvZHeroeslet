using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildBerry : Card
{

	protected override IEnumerator OnThisPlay()
	{
        for (int i = 0; i < Tile.ROWS; i++) for (int j = 0; j < Tile.COLUMNS; j++)
        {
            if (!(i == row && j == col) && Tile.CanPlantInCol(j, Tile.GetTeamTiles(team), false, false))
            {
                choices.Add(Tile.GetTeamTiles(team)[i, j].GetComponent<BoxCollider2D>());
            }
        }
        if (choices.Count > 0)
        {
            yield return Glow();
            var choice = choices[Random.Range(0, choices.Count)];
            yield return SyncRandomChoiceAcrossNetwork(choice.GetComponent<Tile>().row + " - " + choice.GetComponent<Tile>().col);
            Move(int.Parse(GameManager.Instance.GetShuffledList()[0]), int.Parse(GameManager.Instance.GetShuffledList()[1]));
        }
		yield return base.OnThisPlay();
	}

}
