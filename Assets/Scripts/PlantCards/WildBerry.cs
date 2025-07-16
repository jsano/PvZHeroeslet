using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildBerry : Card
{

	protected override IEnumerator OnThisPlay()
	{
        if (team == GameManager.Instance.team)
        {
            for (int i = 0; i < 2; i++) for (int j = 0; j < 5; j++)
            {
                if (!(i == row && j == col) && Tile.CanPlantInCol(j, Tile.plantTiles, false, false))
                {
                    choices.Add(Tile.plantTiles[i, j].GetComponent<BoxCollider2D>());
                }
            }
            for (int n = choices.Count - 1; n > 0; n--)
            {
                int k = Random.Range(0, n + 1);
                var temp = choices[n];
                choices[n] = choices[k];
                choices[k] = temp;
            }
            if (choices.Count > 0) GameManager.Instance.StoreRpc(choices[0].GetComponent<Tile>().row + " - " + choices[0].GetComponent<Tile>().col);
        }
        
        yield return new WaitForSeconds(1);
        if (GameManager.Instance.shuffledList != null) Move(int.Parse(GameManager.Instance.shuffledList[0]), int.Parse(GameManager.Instance.shuffledList[1]));
		yield return base.OnThisPlay();
	}

}
