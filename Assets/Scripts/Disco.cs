using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disco : Card
{

	protected override IEnumerator OnThisPlay()
	{
		if (GameManager.Instance.team == team)
		{
			GameManager.Instance.DisableHandCards();
			for (int col = 0; col < 5; col++)
			{
				if (Tile.zombieTiles[0, col].planted == null)
				{
					choices.Add(Tile.zombieTiles[0, col].GetComponent<BoxCollider2D>());
				}
			}
			if (choices.Count == 1) yield return OnSelection(choices[0]);
			if (choices.Count >= 2)
			{
				selecting = true;
				yield return new WaitUntil(() => selecting == false);
			}
		}
		yield return base.OnThisPlay();
	}

	protected override IEnumerator OnSelection(BoxCollider2D bc)
	{
		//yield return new WaitForSeconds(1);
		GameManager.Instance.PlayCardRpc(HandCard.MakeDefaultFS(36), bc.GetComponent<Tile>().row, bc.GetComponent<Tile>().col, true);
		yield return null;
	}

}
