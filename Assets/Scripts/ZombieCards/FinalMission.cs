using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalMission : Card
{

	protected override IEnumerator OnThisPlay()
	{
		for (int row = 0; row < 2; row++)
		{
			for (int col = 0; col < 5; col++)
			{
				if (Tile.plantTiles[row, col].planted != null && Tile.plantTiles[row, col].planted != this)
				{
					choices.Add(Tile.plantTiles[row, col].GetComponent<BoxCollider2D>());
				}
			}
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
        yield return new WaitForSeconds(1);
        Tile.zombieTiles[row, col].planted.Destroy();
		Card c = bc.GetComponent<Tile>().planted;
        yield return c.ReceiveDamage(4, this);
    }

    public override bool IsValidTarget(BoxCollider2D bc)
    {
        Tile t = bc.GetComponent<Tile>();
        if (t == null) return false;
        if (t.HasRevealedPlanted() && t.planted.team == Team.Zombie) return true;
        return false;
    }

}
