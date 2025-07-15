using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBalloons : Card
{
    int val = -2;

	protected override IEnumerator OnThisPlay()
	{
		yield return new WaitForSeconds(1);
        if (GameManager.Instance.team == team) 
        {
            if (GameManager.Instance.remainingTop >= 6) val--;
        }
        else if (GameManager.Instance.opponentRemainingTop >= 6) val--;

        Tile.zombieTiles[row, col].planted.ChangeStats(val, val);
		yield return base.OnThisPlay();
	}

    public override bool IsValidTarget(BoxCollider2D bc)
	{
        if (!base.IsValidTarget(bc)) return false;
        Tile t = bc.GetComponent<Tile>();
		if (t == null) return false;
		if (t.HasRevealedPlanted() && t.planted.team == Team.Zombie && !t.planted.amphibious) return true;
		return false;
	}
}
