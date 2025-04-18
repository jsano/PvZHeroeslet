using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RescueRadish : Card
{

	protected override IEnumerator OnThisPlay()
	{
		for (int row = 0; row < 2; row++)
		{
			for (int col = 0; col < 5; col++)
			{
				if (Tile.plantTiles[row, col].planted != null && Tile.plantTiles[row, col].planted != this)
				{
					choices.Add(Tile.plantTiles[row, col].planted.GetComponent<BoxCollider2D>());
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
		bc.GetComponent<Card>().Bounce();
		GameManager.Instance.EndSelectingRpc();
    }

}
