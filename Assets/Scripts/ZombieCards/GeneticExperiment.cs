using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GeneticExperiment : Card
{

	public override IEnumerator OnZombieTricks()
	{
        if (col > 0 && Tile.zombieTiles[0, col - 1].HasRevealedPlanted() || col < 4 && Tile.zombieTiles[0, col + 1].HasRevealedPlanted())
        {
            //yield return new WaitForSeconds(1);
            ChangeStats(1, 1);
        }
		
        yield return base.OnZombieTricks();
	}

}
