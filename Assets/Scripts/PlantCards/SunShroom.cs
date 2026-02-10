using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunShroom : Card
{

	protected override IEnumerator OnTurnStart()
	{
        Tile.GetTeamTiles(team)[row, col].Unplant(true);
        yield return Glow();
        yield return GameManager.Instance.UpdateRemaining(1, team);
        Card c = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Sunnier-shroom")]);
        Tile.GetTeamTiles(team)[row, col].Plant(c);
        Destroy(gameObject);
    }

}
