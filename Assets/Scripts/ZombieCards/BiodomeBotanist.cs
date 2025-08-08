using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiodomeBotanist : Card
{

	protected override IEnumerator OnThisPlay()
	{
		for (int col = 0; col < 4; col++)
		{
			if (Tile.CanPlantInCol(col, Tile.plantTiles, false, false))
			{
				choices.Add(Tile.plantTiles[0, col].GetComponent<BoxCollider2D>());
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
        string[] options = new string[] { "Weenie Beanie", "Peashooter", "Button Mushroom", "Bellflower", "Small-nut" };
		yield return SyncRandomChoiceAcrossNetwork(options[UnityEngine.Random.Range(0, options.Length)]);
		Card c = Instantiate(AllCards.Instance.cards[AllCards.NameToID(GameManager.Instance.GetShuffledList()[0])]);
		Tile.zombieTiles[0, t.col].Plant(c);
    }

}
