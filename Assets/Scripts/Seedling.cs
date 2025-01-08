using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seedling : Card
{

	protected override IEnumerator OnTurnStart()
	{
		yield return new WaitForSeconds(1);
		if (GameManager.Instance.team == team)
		{
			int newCard = AllCards.RandomFromCost(Team.Plant, 1, 0, 1, 2, 3, 4, 5, 6);
			Card c = AllCards.Instance.cards[newCard];
			GameManager.Instance.PlayCardRpc(new HandCard.FinalStats()
			{
				hp = c.HP,
				atk = c.atk,
				abilities = "",
				ID = newCard,
				cost = c.cost,
			}, row, col);
		}
		Destroy(gameObject);
	}

}
