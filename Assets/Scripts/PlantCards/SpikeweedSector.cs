using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeweedSector : Card
{

	public override IEnumerator BeforeCombat()
	{
		if (Tile.zombieTiles[0, col].HasRevealedPlanted())
		{
			yield return new WaitForSeconds(1);
			yield return Tile.zombieTiles[0, col].planted.ReceiveDamage(2, this);
		}
		yield return base.BeforeCombat();
	}

}
