using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunStrike : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		for (int i = 0; i < 2; i++) for (int j = 0; j < 5; j++) if (Tile.plantTiles[i, j].HasRevealedPlanted())
				{
					Tile.plantTiles[i, j].planted.strikethrough += 1;
					GameManager.Instance.removeStrikethrough.Add(Tile.plantTiles[i, j].planted);
				}
		yield return GameManager.Instance.GainHandCard(team, AllCards.RandomTrick(team));
		yield return base.OnThisPlay();
	}

}
