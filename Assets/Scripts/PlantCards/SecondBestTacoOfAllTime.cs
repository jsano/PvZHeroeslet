using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondBestTacoOfAllTime : Card
{
	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		if (row == -1 && col == -1) yield return GameManager.Instance.plantHero.Heal(4);
		else yield return Tile.plantTiles[row, col].planted.Heal(4);
		yield return GameManager.Instance.DrawCard(team);
		yield return base.OnThisPlay();
	}

	public override bool IsValidTarget(BoxCollider2D bc)
	{
		Tile t = bc.GetComponent<Tile>();
		if (t != null)
		{
			if (t.HasRevealedPlanted() && t.planted.team == Team.Plant && t.planted.isDamaged()) return true;
		}
		else
		{
			if (bc.GetComponent<Hero>().team == Team.Plant && bc.GetComponent<Hero>().isDamaged()) return true;
		}
		return false;
	}

}
