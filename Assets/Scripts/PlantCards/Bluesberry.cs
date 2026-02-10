using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bluesberry : Card
{

	protected override IEnumerator OnThisPlay()
	{
        for (int col = 0; col < Tile.COLUMNS; col++)
        {
            if (Tile.GetTeamTiles(GetOpponent(team))[0, col].HasRevealedPlanted()) choices.Add(Tile.GetTeamTiles(GetOpponent(team))[0, col].GetComponent<BoxCollider2D>());
        }
        choices.Add(GameManager.Instance.GetTeamHero(GetOpponent(team)).GetComponent<BoxCollider2D>());
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
        Tile t = bc.GetComponent<Tile>();
        if (t == null)
        {
            yield return Glow();
            yield return AttackFX(GameManager.Instance.GetTeamHero(GetOpponent(team)));
            yield return GameManager.Instance.GetTeamHero(GetOpponent(team)).ReceiveDamage(2, this);
        }
        else
        {
            yield return Glow();
            yield return AttackFX(t.planted);
            yield return t.planted.ReceiveDamage(2, this);
        }
    }

}
