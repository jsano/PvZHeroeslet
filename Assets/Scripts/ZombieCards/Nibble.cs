using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nibble : Card
{
	protected override IEnumerator OnThisPlay()
	{
        yield return Glow();
        Tile.plantTiles[row, col].planted.ChangeStats(-1, -1);
		yield return GameManager.Instance.zombieHero.Heal(2);
		yield return base.OnThisPlay();
	}

	public override bool IsValidTarget(BoxCollider2D bc)
	{
        if (!base.IsValidTarget(bc)) return false;
        Tile t = bc.GetComponent<Tile>();
		if (t == null) return false;
		if (t.HasRevealedPlanted() && t.planted.team == Team.Plant) return true;
		return false;
	}

}
