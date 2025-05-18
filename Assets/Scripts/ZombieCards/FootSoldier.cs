using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSoldier : Card
{

	protected override IEnumerator OnThisPlay()
	{
        if (col == 0)
        {
            for (int col = 0; col < 5; col++)
            {
                for (int row = 0; row < 2; row++) if (Tile.plantTiles[row, col].HasRevealedPlanted()) choices.Add(Tile.plantTiles[row, col].GetComponent<BoxCollider2D>());
            }
            choices.Add(GameManager.Instance.plantHero.GetComponent<BoxCollider2D>());
            if (choices.Count == 1) yield return OnSelection(choices[0]);
            if (choices.Count >= 2)
            {
                if (GameManager.Instance.team == team) selected = false;
                yield return new WaitUntil(() => GameManager.Instance.selection != null);
                yield return OnSelection(GameManager.Instance.selection);
            }
        }
        yield return base.OnThisPlay();
	}

	protected override IEnumerator OnSelection(BoxCollider2D bc)
	{
        yield return base.OnSelection(bc);
        Tile t = bc.GetComponent<Tile>();
        if (t == null)
        {
            yield return AttackFX(GameManager.Instance.plantHero);
            yield return GameManager.Instance.plantHero.ReceiveDamage(1, this);
        }
        else
        {
            yield return AttackFX(t.planted);
            yield return t.planted.ReceiveDamage(1, this);
        }
    }

}
