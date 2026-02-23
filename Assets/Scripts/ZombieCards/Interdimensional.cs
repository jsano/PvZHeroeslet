using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interdimensional : Card
{

	protected override IEnumerator OnCardPlay(Card played)
	{
		if (played != this && played.tribes.Contains(Tribe.Science))
		{
            Tile.GetTeamTiles(team)[0, col].Unplant(true);
            yield return Glow();
            yield return SyncRandomChoiceAcrossNetwork(AllCards.RandomFromCost((3, 3), true) + "");
            Card c = Instantiate(AllCards.Instance.cards[int.Parse(GameManager.Instance.GetShuffledList()[0])]);
            Tile.GetTeamTiles(team)[0, col].Plant(c);
            yield return null;
            Destroy(gameObject);
        }
	}

}
