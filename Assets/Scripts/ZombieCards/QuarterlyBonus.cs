using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuarterlyBonus : Card
{

	protected override IEnumerator OnThisPlay()
	{
		Card c = Tile.zombieTiles[row, col].planted;
        yield return new WaitForSeconds(1);
        c.ChangeStats(-c.baseAtk + 4, 0);
		yield return c.BonusAttack();
		yield return base.OnThisPlay();
	}

    public override bool IsValidTarget(BoxCollider2D bc)
    {
        if (!base.IsValidTarget(bc)) return false;
        Tile t = bc.GetComponent<Tile>();
        if (t == null) return false;
        Card c = t.planted;
        if (c == null) return false;
        if (c.team == Team.Zombie) return true;
        return false;
    }

}
