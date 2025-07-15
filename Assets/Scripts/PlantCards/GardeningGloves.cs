using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardeningGloves : Card
{

	protected override IEnumerator OnThisPlay()
	{
		Card chosen = Tile.plantTiles[row, col].planted;
        for (int i = 0; i < 2; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				if (!(i == chosen.row && j == chosen.col) && Tile.CanPlantInCol(j, Tile.plantTiles, chosen.teamUp, chosen.amphibious))
				{
					choices.Add(Tile.plantTiles[i, j].GetComponent<BoxCollider2D>());
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
		Tile t = bc.GetComponent<Tile>();
		Tile.plantTiles[row, col].planted.Move(t.row, t.col);
        yield return GameManager.Instance.GainHandCard(team, AllCards.RandomTrick(team));
    }

	public override bool IsValidTarget(BoxCollider2D bc)
	{
        if (!base.IsValidTarget(bc)) return false;
        Tile t = bc.GetComponent<Tile>();
		if (t == null) return false;
        if (t.HasRevealedPlanted() && t.planted.team == Team.Plant) return true;
        return false;
    }

}
