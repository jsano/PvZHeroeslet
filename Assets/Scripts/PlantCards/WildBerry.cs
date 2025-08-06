using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildBerry : Card
{

	protected override IEnumerator OnThisPlay()
	{
        for (int i = 0; i < 2; i++) for (int j = 0; j < 5; j++)
        {
            if (!(i == row && j == col) && Tile.CanPlantInCol(j, Tile.plantTiles, false, false))
            {
                choices.Add(Tile.plantTiles[i, j].GetComponent<BoxCollider2D>());
            }
        }
        if (choices.Count > 0)
        {
            yield return new WaitForSeconds(1);
            var choice = choices[Random.Range(0, choices.Count)];
            yield return SyncRandomChoiceAcrossNetwork(choice.GetComponent<Tile>().row + " - " + choice.GetComponent<Tile>().col);
            Move(int.Parse(GameManager.Instance.shuffledLists[^1][0]), int.Parse(GameManager.Instance.shuffledLists[^1][1]));
        }
		yield return base.OnThisPlay();
	}

}
