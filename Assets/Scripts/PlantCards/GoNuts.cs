using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoNuts : Card
{

	protected override IEnumerator OnCardPlay(Card played)
	{
		if (played != this)
		{
			if (played.teamUp)
			{
                yield return Glow();
                for (int i = 0; i < 2; i++) for (int col = 0; col < 5; col++)
				{
					Card c = Tile.plantTiles[i, col].planted;
					if (c != null && c.teamUp)
					{
						c.ChangeStats(1, 0);
					}
				}
			}
		}
		yield return base.OnCardPlay(played);
	}

}
