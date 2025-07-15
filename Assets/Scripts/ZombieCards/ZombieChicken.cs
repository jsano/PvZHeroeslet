using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieChicken : Card
{

	protected override IEnumerator OnCardPlay(Card played)
	{
		if (played.col == col && played.team == Team.Plant)
		{
			if (GameManager.Instance.team == team)
			{
                for (int j = 0; j < 4; j++)
                {
                    if (j != col && Tile.zombieTiles[row, j].planted == null)
                    {
                        choices.Add(Tile.zombieTiles[row, j].GetComponent<BoxCollider2D>());
                    }
                }
                for (int n = choices.Count - 1; n > 0; n--)
                {
                    int k = Random.Range(0, n + 1);
                    var temp = choices[n];
                    choices[n] = choices[k];
                    choices[k] = temp;
                }
                GameManager.Instance.StoreRpc(choices[0].GetComponent<Tile>().row + " - " + choices[0].GetComponent<Tile>().col);
            }
			yield return new WaitForSeconds(1);
            Move(int.Parse(GameManager.Instance.shuffledList[0]), int.Parse(GameManager.Instance.shuffledList[1]));
        }
		yield return base.OnCardPlay(played);
	}

}
