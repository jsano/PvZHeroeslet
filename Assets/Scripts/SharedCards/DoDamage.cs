using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoDamage : Card
{

	public bool includeHero;
	public int damage;

	protected override IEnumerator OnThisPlay()
	{
        yield return new WaitForSeconds(1);
		if (row == -1 && col == -1) yield return GameManager.Instance.GetTeamHero(GetOpponent(GameManager.Instance.team)).ReceiveDamage(damage, this);
		else yield return Tile.GetTeamTiles(GetOpponent(GameManager.Instance.team))[row, col].planted.ReceiveDamage(damage, this);
		yield return base.OnThisPlay();
	}

	public override bool IsValidTarget(BoxCollider2D bc)
	{
		if (!base.IsValidTarget(bc)) return false;
		Tile t = bc.GetComponent<Tile>();
		if (t != null)
		{
			if (t.HasRevealedPlanted() && t.planted.team == GetOpponent(GameManager.Instance.team)) return true;
			return false;
		}
		else
		{
			if (includeHero && bc.GetComponent<Hero>().team == GetOpponent(GameManager.Instance.team)) return true;
		}
		return false;
	}

}
