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
			Card temp = localAllyTiles[0, col].planted;
			if (temp != null && temp.traits.Contains(Trait.Pea))
			{
				temp.RaiseAttack(2);
				buffing = temp;
			}
		}
		else if (buffing == null && played.col == col && played.row == 0 && played.traits.Contains(Trait.Pea))
		{
			played.RaiseAttack(2);
			buffing = played;
		}
		yield return null;
	}

	protected override IEnumerator OnCardDeath(Card died)
	{
		if (died == this) if (buffing != null) buffing.RaiseAttack(-2);
		if (died == buffing) buffing = null;
		yield return null;
	}

	protected override IEnumerator OnCardMoved(Card moved)
	{
		if (moved == this)
		{
			if (buffing != null) buffing.RaiseAttack(-2);
			buffing = null;
			StartCoroutine(OnCardPlay(localAllyTiles[1 - row, col].planted));
		}
		if (moved == buffing)
		{
			moved.RaiseAttack(-2);
			buffing = null;
		}
		yield return null;
	}

}