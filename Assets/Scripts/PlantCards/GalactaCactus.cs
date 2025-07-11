using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalactaCactus : Card
{

	protected override IEnumerator OnCardDeath(Card died)
	{
		if (died == this)
		{
            List<Damagable> targets = new();

			targets.Add(GameManager.Instance.plantHero);
            targets.Add(GameManager.Instance.zombieHero);
            for (int i = 0; i < 2; i++) for (int j = 0; j < 5; j++)
				{
					if (Tile.plantTiles[i, j].planted != null) targets.Add(Tile.plantTiles[i, j].planted);
                    if (Tile.zombieTiles[i, j].planted != null && Tile.zombieTiles[i, j].planted != this) targets.Add(Tile.zombieTiles[i, j].planted);
                }
			yield return AttackFXs(targets);
			foreach (Damagable d in targets) StartCoroutine(d.ReceiveDamage(1, this));
        }
		yield return base.OnCardDeath(died);
	}

}
