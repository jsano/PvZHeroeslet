using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Torchwood : Card
{

	private Card buffing;

	protected override IEnumerator OnCardPlay(Card played)
	{
		if (played == this)
		{
			Card temp = Tile.tileObjects[0, col].planted;
			if (temp != null && temp.traits.Contains(Trait.Pea))
			{
				GameManager.Instance.RaiseAttackRpc(team, 0, col, 2);
				buffing = temp;
			}
		}
		else if (buffing == null && played.col == col && played.row == 0 && played.traits.Contains(Trait.Pea))
		{
			GameManager.Instance.RaiseAttackRpc(team, played.row, played.col, 2);
			buffing = played;
		}
		yield return null;
	}

	protected override IEnumerator OnCardDeath(Card died)
	{
		if (died == this) if (buffing != null) GameManager.Instance.RaiseAttackRpc(team, buffing.row, buffing.col, -2);
		if (died == buffing) buffing = null;
		yield return null;
	}

	protected override IEnumerator OnCardMoved(Card moved)
	{
		if (moved == this)
		{
			if (buffing != null) GameManager.Instance.RaiseAttackRpc(team, buffing.row, buffing.col, -2);
			buffing = null;
			StartCoroutine(OnCardPlay(Tile.tileObjects[1 - row, col].planted));
		}
		if (moved == buffing)
		{
			GameManager.Instance.RaiseAttackRpc(team, moved.row, moved.col, -2);
			buffing = null;
		}
		yield return null;
	}

}