using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Torchwood : Card
{

	private Card buffing;

    protected override void OnCardPlayImmediate(Card played)
    {
		if (played == this)
		{
			Card temp = Tile.GetTeamTiles(team)[0, col].planted;
			if (temp != null && temp.tribes.Contains(Tribe.Pea))
			{
				temp.ChangeStats(2, 0);
				buffing = temp;
			}
		}
		else if (buffing == null && played.col == col && played.row == 0 && played.tribes.Contains(Tribe.Pea) && played.team == team)
		{
			played.ChangeStats(2, 0);
			buffing = played;
		}
	}

	protected override IEnumerator OnCardDeath(Tuple<Card, Card> died)
	{
		if (died.Item1 == this) if (buffing != null) buffing.ChangeStats(-2, 0);
		if (died.Item1 == buffing) buffing = null;
		yield return base.OnCardDeath(died);
	}

	protected override IEnumerator OnCardMoved(Card moved)
	{
		if (moved == this)
		{
			if (buffing != null) buffing.ChangeStats(-2, 0);
			buffing = null;
			StartCoroutine(OnCardPlay(Tile.GetTeamTiles(team)[1 - row, col].planted));
		}
		if (moved == buffing)
		{
			moved.ChangeStats(-2, 0);
			buffing = null;
		}
		yield return base.OnCardMoved(moved);
	}

    void OnDestroy()
    {
		if (died) return;
        if (buffing != null) buffing.ChangeStats(-2, 0);
    }

}