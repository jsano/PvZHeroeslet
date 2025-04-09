using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medic : Card
{

	protected override IEnumerator OnThisPlay()
	{
		for (int row = 0; row < 2; row++)
		{
			for (int col = 0; col < 5; col++)
			{
				if (Tile.zombieTiles[row, col].planted != null && Tile.zombieTiles[row, col].planted.isDamaged())
				{
					choices.Add(Tile.zombieTiles[row, col].planted.GetComponent<BoxCollider2D>());
				}
			}
		}
		if (GameManager.Instance.team == team)
		{			
			if (GameManager.Instance.zombieHero.isDamaged()) choices.Add(GameManager.Instance.zombieHero.GetComponent<BoxCollider2D>());
			if (choices.Count == 1) StartCoroutine(OnSelection(choices[0]));
			if (choices.Count >= 2)
			{
				selected = false;
			}
		}
        if (choices.Count > 0) GameManager.Instance.selecting = true;
		yield return base.OnThisPlay();
	}

	protected override IEnumerator OnSelection(BoxCollider2D bc)
	{
		yield return new WaitForSeconds(1);
		Card c = bc.GetComponent<Card>();
		if (c == null) GameManager.Instance.HealRpc(team, -1, -1, 4, false);
		else GameManager.Instance.HealRpc(team, c.row, c.col, 4, false);
		GameManager.Instance.EndSelectingRpc();
    }

}