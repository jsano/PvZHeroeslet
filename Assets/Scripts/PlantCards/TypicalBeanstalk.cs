using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TypicalBeanstalk : Card
{

	protected override IEnumerator OnThisPlay()
	{
        for (int i = 0; i < 2; i++) if (col > 0 && Tile.plantTiles[i, col - 1].HasRevealedPlanted() && Tile.plantTiles[i, col - 1].planted.tribes.Contains(Tribe.Leafy) ||
            col < 4 && Tile.plantTiles[i, col + 1].HasRevealedPlanted() && Tile.plantTiles[i, col + 1].planted.tribes.Contains(Tribe.Leafy))
        {
            yield return Glow();
            ChangeStats(0, 1);
            yield return GameManager.Instance.GainHandCard(team, AllCards.RandomFromTribe((Tribe.Leafy, Tribe.Leafy)));
            break;
        }
		
        yield return base.OnThisPlay();
	}

}
