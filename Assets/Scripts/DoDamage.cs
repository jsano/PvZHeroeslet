using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoDamage : Card
{

	public bool includeHero;
	public Team targetTeam;
	public int damage;

	protected override IEnumerator OnThisPlay()
	{
		var hero = targetTeam == Team.Plant ? GameManager.Instance.plantHero : GameManager.Instance.zombieHero;
		var tiles = targetTeam == Team.Plant ? Tile.plantTiles : Tile.zombieTiles;
        yield return new WaitForSeconds(1);
		if (row == -1 && col == -1) yield return hero.ReceiveDamage(damage, this);
		else yield return tiles[row, col].planted.ReceiveDamage(damage, this);
		yield return base.OnThisPlay();
	}

	public override bool IsValidTarget(BoxCollider2D bc)
	{
		if (!base.IsValidTarget(bc)) return false;
		Tile t = bc.GetComponent<Tile>();
		if (t != null)
		{
			if (t.HasRevealedPlanted() && t.planted.team == targetTeam) return true;
			return false;
		}
		else
		{
			if (includeHero && bc.GetComponent<Hero>().team == targetTeam) return true;
		}
		return false;
	}

}
