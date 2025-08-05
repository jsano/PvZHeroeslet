using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FraidyCat : Card
{

	protected override IEnumerator OnCardPlay(Card played)
	{
        choices.Clear();
        if (played.type == Type.Trick && played.team == Team.Plant)
        {
            for (int j = 0; j < 4; j++)
            {
                if (j != col && Tile.zombieTiles[row, j].planted == null)
                {
                    choices.Add(Tile.zombieTiles[row, j].GetComponent<BoxCollider2D>());
                }
            }
            if (GameManager.Instance.team == team)
			{    
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
            if (choices.Count > 0)
            {
                yield return new WaitUntil(() => GameManager.Instance.shuffledList != null);
                Move(int.Parse(GameManager.Instance.shuffledList[0]), int.Parse(GameManager.Instance.shuffledList[1]));
            }
            ChangeStats(1, 1);
        }
		yield return base.OnCardPlay(played);
	}

}
