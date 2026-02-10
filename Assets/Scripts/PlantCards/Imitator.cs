using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Imitator : Card
{

	protected override IEnumerator OnCardPlay(Card played)
	{
		if (played != this && played.type == Type.Unit && played.team == team)
		{
            Tile.GetTeamTiles(team)[row, col].Unplant(true);
            yield return Glow();
            Card c = Instantiate(AllCards.InstanceToPrefab(played));
            Tile.GetTeamTiles(team)[row, col].Plant(c);
            Destroy(gameObject);
        }
    }

}
