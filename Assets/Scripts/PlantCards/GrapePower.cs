using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapePower : Card
{

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		Tile.plantTiles[row, col].planted.ChangeStats(Tile.plantTiles[row, col].planted.atk - Tile.plantTiles[row, col].planted.antihero, 0);
		yield return GameManager.Instance.GainHandCard(team, AllCards.NameToID("Grape Responsibility"));
        yield return base.OnThisPlay();
	}

	public override bool IsValidTarget(BoxCollider2D bc)
	{
        if (!base.IsValidTarget(bc)) return false;
        Tile t = bc.GetComponent<Tile>();
		if (t == null) return false;
		Card c = t.planted;
		if (c == null) return false;
		if (c.team == Team.Plant) return true;
		return false;
	}

}
