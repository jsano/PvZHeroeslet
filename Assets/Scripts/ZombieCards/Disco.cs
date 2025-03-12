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
			for (int col = 0; col < 4; col++)
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
				yield return new WaitUntil(() => selected == true);
			}
		}
		yield return base.OnThisPlay();
	}

	protected override IEnumerator OnSelection(BoxCollider2D bc)
	{
		//yield return new WaitForSeconds(1);
		Tile t = bc.GetComponent<Tile>();
		GameManager.Instance.PlayCardRpc(HandCard.MakeDefaultFS(AllCards.NameToID("Backup Dancer")), t.row, t.col, true);
        yield return new WaitUntil(() => t.planted != null && t.planted.playState == PlayState.OnThisPlayed);
        selected = true;
	}

}
