using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretAgent : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		FinalStats fs = new(AllCards.NameToID(AllCards.InstanceToPrefab(Tile.zombieTiles[row, col].planted).name));
		fs.atk += 3;
		fs.hp += 3;
		Tile.zombieTiles[row, col].planted.Bounce(fs);
		yield return base.OnThisPlay();
	}

	public override bool IsValidTarget(BoxCollider2D bc)
	{
        if (!base.IsValidTarget(bc)) return false;
        Tile t = bc.GetComponent<Tile>();
		if (t == null) return false;
		if (t.HasRevealedPlanted() && t.planted.team == Team.Zombie) return true;
		return false;
	}

}