using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieChicken : Card
{

	protected override IEnumerator OnCardPlay(Card played)
	{
        choices.Clear();
		if (played.col == col && played.team == Team.Plant && played.type == Type.Unit)
		{
            for (int j = 0; j < 4; j++)
            {
                if (j != col && Tile.zombieTiles[row, j].planted == null)
                {
                    choices.Add(Tile.zombieTiles[row, j].GetComponent<BoxCollider2D>());
                }
            }
            if (choices.Count > 0)
            {
                yield return new WaitForSeconds(1);
                var choice = choices[Random.Range(0, choices.Count)];
                yield return SyncRandomChoiceAcrossNetwork(choice.GetComponent<Tile>().row + " - " + choice.GetComponent<Tile>().col);
                Move(int.Parse(GameManager.Instance.shuffledLists[^1][0]), int.Parse(GameManager.Instance.shuffledLists[^1][1]));
            }
        }
		yield return base.OnCardPlay(played);
	}

}
