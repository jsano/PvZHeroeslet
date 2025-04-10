using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayinAlive : Card
{
	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		StartCoroutine(Tile.plantTiles[row, col].planted.ReceiveDamage(3, this));
		GameManager.Instance.zombieHero.Heal(3);
		yield return base.OnThisPlay();
	}

	public override bool IsValidTarget(BoxCollider2D bc)
	{
		Tile t = bc.GetComponent<Tile>();
		if (t == null) return false;
		if (t.planted != null && t.planted.team == Team.Plant) return true;
		return true;
	}

}
