using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnthawedViking : Card
{

	protected override IEnumerator OnThisPlay()
	{
		if (evolved)
		{
            yield return new WaitForSeconds(1);
            ChangeStats(1, 1);
			for (int col = 0; col < 5; col++)
			{
				for (int row = 0; row < 2; row++)
				{
					if (Tile.plantTiles[row, col].planted != null) Tile.plantTiles[row, col].planted.Freeze();
				}
			}
		}
		else
		{
			yield return new WaitForSeconds(1);
			for (int row = 0; row < 2; row++)
			{
				if (Tile.plantTiles[row, col].planted != null) Tile.plantTiles[row, col].planted.Freeze();
			}
		}
        yield return base.OnThisPlay();
	}

}
