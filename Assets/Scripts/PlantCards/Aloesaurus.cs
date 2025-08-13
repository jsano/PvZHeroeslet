using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aloesaurus : Card
{

	protected override IEnumerator OnCardDraw(Team team)
	{
		if (team == this.team)
		{
            yield return Glow();
            StartCoroutine(GameManager.Instance.plantHero.Heal(1));
			for (int i = 0; i < 2; i++) for (int j = 0; j < 5; j++) if (Tile.plantTiles[i, j].HasRevealedPlanted()) StartCoroutine(Tile.plantTiles[i, j].planted.Heal(1));
		}
		yield return base.OnCardDraw(team);
	}

}
