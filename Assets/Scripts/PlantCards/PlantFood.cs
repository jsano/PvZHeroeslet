using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantFood : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		Tile.plantTiles[row, col].planted.RaiseAttack(1);
		Tile.plantTiles[row, col].planted.Heal(1, true);
        yield return Tile.plantTiles[row, col].planted.Attack();
		yield return base.OnThisPlay();
	}

	public override bool IsValidTarget(BoxCollider2D bc)
	{
		Tile t = bc.GetComponent<Tile>();
		if (t == null) return false;
        if (t.HasRevealedPlanted() && t.planted.team == Team.Plant) return true;
        return false;
    }

}
