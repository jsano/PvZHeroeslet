using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medic : Card
{

	protected override IEnumerator OnThisPlay()
	{
		for (int row = 0; row < 2; row++)
		{
			for (int col = 0; col < 5; col++)
			{
				if (Tile.zombieTiles[row, col].planted != null && Tile.zombieTiles[row, col].planted.isDamaged())
				{
					choices.Add(Tile.zombieTiles[row, col].GetComponent<BoxCollider2D>());
				}
			}
		}
        choices.Add(GameManager.Instance.zombieHero.GetComponent<BoxCollider2D>());
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
        yield return new WaitForSeconds(1);
		Tile t = bc.GetComponent<Tile>();
		if (t == null) yield return GameManager.Instance.zombieHero.Heal(4);
		else yield return t.planted.Heal(4);
    }

}