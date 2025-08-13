using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zookeeper : Card
{

	protected override IEnumerator OnCardPlay(Card played)
	{
		if (played != this)
		{
			if (played.tribes.Contains(Tribe.Pet))
			{
                yield return Glow();
                for (int col = 0; col < 5; col++)
				{
					Card c = Tile.zombieTiles[0, col].planted;
					if (c != null && c.tribes.Contains(Tribe.Pet))
					{
						c.ChangeStats(1, 0);
					}
				}
			}
		}
		yield return base.OnCardPlay(played);
	}

}
