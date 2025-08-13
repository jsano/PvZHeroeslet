using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Headseeker : Card
{

    protected override IEnumerator OnThisPlay()
    {
        if (evolved)
        {
            yield return Glow();
            bullseye += 1;
            ChangeStats(2, 2);
        }
        yield return base.OnThisPlay();
    }

    protected override IEnumerator OnCardPlay(Card played)
	{
		if (played != this && played.tribes.Contains(Tribe.Dancing))
		{
            yield return Glow();
            yield return AttackFX(Tile.plantHeroTiles[col]);
            yield return Tile.plantHeroTiles[col].ReceiveDamage(2, this, bullseye > 0);
		}
		yield return base.OnCardPlay(played);
	}

}
