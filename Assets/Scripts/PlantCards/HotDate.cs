using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotDate : Card
{

	protected override IEnumerator OnThisPlay()
	{
        for (int i = 0; i < Tile.ROWS; i++) for (int col = 0; col < Tile.COLUMNS; col++)
        {
            if (Tile.GetTeamTiles(GetOpponent(team))[i, col].planted != null && col == this.col) 
            {
                yield return base.OnThisPlay();
                yield break;
            }
            if (Tile.GetTeamTiles(GetOpponent(team))[i, col].HasRevealedPlanted()) choices.Add(Tile.GetTeamTiles(GetOpponent(team))[i, col].GetComponent<BoxCollider2D>());
        }
        if (choices.Count == 1) yield return OnSelection(choices[0]);
        if (choices.Count >= 2)
        {
            if (GameManager.Instance.team == team) selected = false;
            yield return new WaitUntil(() => GameManager.Instance.selection != null);
            yield return OnSelection(GameManager.Instance.selection);
        }
        yield return base.OnThisPlay();
	}

	protected override IEnumerator OnSelection(BoxCollider2D bc)
	{
        yield return base.OnSelection(bc);
        yield return Glow();
		Card c = bc.GetComponent<Tile>().planted;
        c.Move(c.row, col);
    }

    protected override IEnumerator OnCardDeath(Tuple<Card, Card> died)
    {
        Damagable target = GetTargets(col)[0];
        if (died.Item1 == this && target.GetComponent<Card>() != null)
        {
            yield return Glow();
            yield return AttackFX(target);
            yield return target.ReceiveDamage(3, this);
        }
        yield return base.OnCardDeath(died);
    }

}
