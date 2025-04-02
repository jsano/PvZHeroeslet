using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Octo : Card
{

	protected override IEnumerator OnThisPlay()
	{
		for (int col = 0; col < 5; col++)
		{
			if (Tile.zombieTiles[0, col].planted == null)
			{
				choices.Add(Tile.zombieTiles[0, col].GetComponent<BoxCollider2D>());
			}
		}
		if (GameManager.Instance.team == team)
		{
			GameManager.Instance.DisableHandCards();

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
		Tile t = bc.GetComponent<Tile>();
		GameManager.Instance.PlayCardRpc(HandCard.MakeDefaultFS(AllCards.NameToID("Octo-pet")), t.row, t.col, true);
	}

	protected override IEnumerator OnCardDeath(Card died)
	{
		if (died == this)
		{
			GameManager.Instance.GainHandCard(team, AllCards.NameToID("Octo"));
		}
		yield return base.OnCardDeath(died);
	}

}
