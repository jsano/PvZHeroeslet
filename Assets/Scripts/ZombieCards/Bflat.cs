using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bflat : Card
{

	protected override IEnumerator OnThisPlay()
	{
        for (int i = 0; i < 2; i++) for (int j = 0; j < 5; j++)
        {
            if (Tile.plantTiles[i, j].planted != null && Tile.plantTiles[i, j].planted.untrickable == 0)
            {
                choices.Add(Tile.plantTiles[i, j].GetComponent<BoxCollider2D>());
            }
        }
        if (choices.Count > 0)
        {
            yield return new WaitForSeconds(1);
            var choice = choices[UnityEngine.Random.Range(0, choices.Count)];
            yield return SyncRandomChoiceAcrossNetwork(choice.GetComponent<Tile>().row + " - " + choice.GetComponent<Tile>().col);
            Tile.plantTiles[int.Parse(GameManager.Instance.GetShuffledList()[0]), int.Parse(GameManager.Instance.GetShuffledList()[1])].planted.Destroy();
        }
		yield return base.OnThisPlay();
	}

}
