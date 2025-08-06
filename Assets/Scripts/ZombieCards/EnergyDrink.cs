using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyDrink : Card
{

    private bool moved = false;

	public override IEnumerator OnZombieTricks()
	{
        choices.Clear();
        if (!moved)
        {
            moved = true;
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
                var choice = choices[UnityEngine.Random.Range(0, choices.Count)];
                yield return SyncRandomChoiceAcrossNetwork(choice.GetComponent<Tile>().row + " - " + choice.GetComponent<Tile>().col);
                Move(int.Parse(GameManager.Instance.shuffledLists[^1][0]), int.Parse(GameManager.Instance.shuffledLists[^1][1]));
            }
            ChangeStats(1, 1);
        }
		yield return base.OnZombieTricks();
	}

    protected override IEnumerator OnTurnEnd()
    {
        moved = false;
        return base.OnTurnEnd();
    }

}
