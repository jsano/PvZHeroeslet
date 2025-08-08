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
            yield return new WaitForSeconds(1);
            if (choices.Count > 0)
            {
                var choice = choices[Random.Range(0, choices.Count)];
                yield return SyncRandomChoiceAcrossNetwork(choice.GetComponent<Tile>().row + " - " + choice.GetComponent<Tile>().col);
                Move(int.Parse(GameManager.Instance.GetShuffledList()[0]), int.Parse(GameManager.Instance.GetShuffledList()[1]));
            }
            ChangeStats(1, 1);
        }
		yield return base.OnCardPlay(played);
	}

}
