using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleUp : Card
{

	protected override IEnumerator OnThisPlay()
	{
		Card chosen = Tile.plantTiles[row, col].planted;
        for (int i = 0; i < 2; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				if (Tile.CanPlantInCol(j, Tile.plantTiles, chosen.teamUp, chosen.amphibious))
				{
					choices.Add(Tile.plantTiles[i, j].GetComponent<BoxCollider2D>());
				}
			}
		}
		if (GameManager.Instance.team == team)
		{
			if (choices.Count == 1) StartCoroutine(OnSelection(choices[0]));
			if (choices.Count >= 2)
			{
				selected = false;
			}
		}
        if (choices.Count > 0) GameManager.Instance.selecting = true;
		yield return base.OnThisPlay();
	}

	protected override IEnumerator OnSelection(BoxCollider2D bc)
	{
		yield return new WaitForSeconds(1);
		Tile t = bc.GetComponent<Tile>();
		GameManager.Instance.HealRpc(team, row, col, 4, true);
		GameManager.Instance.MoveRpc(team, row, col, t.row, t.col);
		GameManager.Instance.EndSelectingRpc();
    }

	public override bool IsValidTarget(BoxCollider2D bc)
	{
		Tile t = bc.GetComponent<Tile>();
		if (t == null) return false;
        if (t.HasRevealedPlanted() && t.planted.team == Team.Plant) return true;
        return false;
    }

}
