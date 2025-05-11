using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdmiralNavyBean : Card
{

	protected override IEnumerator OnCardPlay(Card played)
	{
		if (played != this && played.tribes.Contains(Tribe.Bean))
		{
			yield return new WaitForSeconds(1);
			yield return Tile.zombieHeroTiles[col].ReceiveDamage(2, this);
		}
		yield return base.OnCardPlay(played);
	}

}
