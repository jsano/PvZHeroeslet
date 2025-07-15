using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuasarWizard : Card
{

	protected override IEnumerator OnThisPlay()
	{
        if (col > 0 && Tile.zombieTiles[0, col - 1].HasRevealedPlanted() || col < 4 && Tile.zombieTiles[0, col + 1].HasRevealedPlanted())
        {
            yield return new WaitForSeconds(1);
            yield return GameManager.Instance.GainHandCard(team, AllCards.RandomFromTribe((Tribe.Superpower, Tribe.Superpower), false, false, team));
        }
		
        yield return base.OnThisPlay();
	}

}
