using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evaporate : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		Tile.plantTiles[row, col].planted.Destroy();
		yield return GameManager.Instance.DrawCard(team);
		yield return base.OnThisPlay();
	}

	public override bool IsValidTarget(BoxCollider2D bc)
	{
		Tile t = bc.GetComponent<Tile>();
		if (t == null) return false;
        if (t.HasRevealedPlanted() && t.planted.team == Team.Plant && t.planted.isDamaged()) return true;
        return false;
	}

}