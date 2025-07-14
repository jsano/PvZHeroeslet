using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interdimensional : Card
{

	protected override IEnumerator OnCardPlay(Card played)
	{
		if (played != this && played.tribes.Contains(Tribe.Science))
		{
            Tile.zombieTiles[0, col].Unplant(true);
            yield return new WaitForSeconds(1);
            if (GameManager.Instance.team == team)
            {
                int newCard = AllCards.RandomFromCost(Team.Zombie, (3, 3), true);
                GameManager.Instance.PlayCardRpc(new FinalStats(newCard), 0, col);
            }
            yield return null;
            Destroy(gameObject);
        }
	}

}
