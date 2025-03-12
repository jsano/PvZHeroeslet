using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerryBlast : Card
{
	protected override IEnumerator OnThisPlay()
	{
		GameManager.Instance.DisableHandCards();
		yield return new WaitForSeconds(1);
		if (row == -1 && col == -1) GameManager.Instance.zombieHero.ReceiveDamage(3);
		else Tile.zombieTiles[row, col].planted.ReceiveDamage(3);
		yield return base.OnThisPlay();
	}

	public override bool IsValidTarget(BoxCollider2D bc)
	{
		Tile t = bc.GetComponent<Tile>();
		if (t != null)
		{
			if (t.planted == null || t.planted.team == Team.Plant) return false;
		}
		else
		{
			if (bc.GetComponent<Hero>().team == Team.Plant) return false;
		}
		return true;
	}

}
