using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportationStation : Card
{

    protected override IEnumerator OnTurnStart()
    {
        if (Tile.GetTeamTiles(team)[0, col].HasRevealedPlanted() || Tile.GetTeamTiles(team)[1, col].HasRevealedPlanted()) yield return GameManager.Instance.GainHandCard(team, AllCards.NameToID("Teleport"));
        yield return base.OnTurnStart();
    }

}
