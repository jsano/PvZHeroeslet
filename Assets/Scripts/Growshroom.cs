using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Growshroom : Card
{

	protected override IEnumerator OnCardPlay(Card played)
	{
		if (played == this && GameManager.Instance.team == team)
		{
			GameManager.Instance.go.interactable = false;
			GameManager.Instance.DisableHandCards();
			for (int row = 0; row < 2; row++)
			{
				for (int col = 0; col < 5; col++)
				{
					if (Tile.tileObjects[row, col].planted != null && Tile.tileObjects[row, col].planted != this)
					{
						choices.Add(Tile.tileObjects[row, col].planted.GetComponent<BoxCollider2D>());
					}
				}
			}
			if (choices.Count == 1) StartCoroutine(OnSelection(choices[0]));
			if (choices.Count >= 2) selecting = true;
		}
		yield return null;
	}

	protected override IEnumerator OnSelection(BoxCollider2D bc)
	{
		yield return new WaitForSeconds(1);
		Card c = bc.GetComponent<Card>();
		GameManager.Instance.RaiseAttackRpc(team, c.row, c.col, 2);
		GameManager.Instance.HealRpc(team, c.row, c.col, 2, true);
		GameManager.Instance.go.interactable = true;
		GameManager.Instance.EnablePlayableHandCards();
	}

}
