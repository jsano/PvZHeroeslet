using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : Card
{

	public int deathDamage;

	protected override IEnumerator OnCardDeath(Card died)
	{
		if (died == this)
		{
			yield return new WaitForSeconds(1);
			if (Tile.zombieTiles[0, col].planted != null) StartCoroutine(Tile.zombieTiles[0, col].planted.ReceiveDamage(deathDamage, this));
			yield return base.OnCardDeath(died);
		} else yield return base.OnCardDeath(died);
    }

}
