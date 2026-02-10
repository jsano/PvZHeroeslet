using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeelShield : Card
{

	protected override IEnumerator OnThisPlay()
	{
        yield return new WaitForSeconds(1);

        for (int i = 0; i < Tile.ROWS; i++) for (int j = 0; j < Tile.COLUMNS; j++) if (Tile.GetTeamTiles(team)[i, j].HasRevealedPlanted()) Tile.GetTeamTiles(team)[i, j].planted.ToggleInvulnerability(true);
        yield return GameManager.Instance.DrawCard(team);

        yield return base.OnThisPlay();
	}

}
