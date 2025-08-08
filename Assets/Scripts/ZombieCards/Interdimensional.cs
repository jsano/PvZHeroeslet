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
            yield return SyncRandomChoiceAcrossNetwork(AllCards.RandomFromCost(Team.Zombie, (3, 3), true) + "");
            Card c = Instantiate(AllCards.Instance.cards[int.Parse(GameManager.Instance.GetShuffledList()[0])]);
            Tile.zombieTiles[0, col].Plant(c);
            yield return null;
            Destroy(gameObject);
        }
	}

}
