using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sunburn : Card
{
	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
		if (row == -1 && col == -1) yield return GameManager.Instance.GetTeamHero(GetOpponent(team)).ReceiveDamage(1, this);
		else yield return Tile.GetTeamTiles(GetOpponent(team))[row, col].planted.ReceiveDamage(1, this);
		int bonus = GameManager.Instance.turn >= 6 ? 2 : 1;
		if (GameManager.Instance.team == team) GameManager.Instance.playerPermanentBonus += bonus;
		else GameManager.Instance.opponentPermanentBonus += bonus;
		yield return base.OnThisPlay();
	}

	public override bool IsValidTarget(BoxCollider2D bc)
	{
        if (!base.IsValidTarget(bc)) return false;
        Tile t = bc.GetComponent<Tile>();
		if (t != null)
		{
			if (!t.HasRevealedPlanted() || t.planted.team == team) return false;
		}
		else
		{
			if (bc.GetComponent<Hero>().team == team) return false;
		}
		return true;
	}

}
