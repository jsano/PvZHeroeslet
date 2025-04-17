using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomRingleader : Card
{
    private int count = -2;

	protected override IEnumerator OnThisPlay()
	{
		for (int row = 0; row < 2;  row++)
		{
			for (int col = 0; col < 5; col++)
			{
				count += Tile.plantTiles[row, col].HasRevealedPlanted() ? 2 : 0;
			}
		}
        if (count <= 0) yield return base.OnThisPlay(); 
		yield return new WaitForSeconds(1);
		RaiseAttack(count);
		yield return base.OnThisPlay();
	}

}
