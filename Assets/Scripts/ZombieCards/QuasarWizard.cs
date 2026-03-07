using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuasarWizard : Card
{

	protected override IEnumerator OnThisPlay()
	{
        if (col > 0 && (Tile.GetTeamTiles(team)[0, col - 1].HasRevealedPlanted() || Tile.GetTeamTiles(team)[1, col - 1].HasRevealedPlanted()) ||
            col < 4 && (Tile.GetTeamTiles(team)[0, col + 1].HasRevealedPlanted() || Tile.GetTeamTiles(team)[1, col + 1].HasRevealedPlanted()))
        {
            yield return Glow();
            yield return GameManager.Instance.GainHandCard(team, AllCards.RandomFromTribe((Tribe.Unique, Tribe.Unique), false, false));
        }
		
        yield return base.OnThisPlay();
	}

}
