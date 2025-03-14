using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pineclone : Card
{

    protected bool suppress;

	protected override IEnumerator OnThisPlay()
	{
        if (suppress) {
            yield return base.OnThisPlay();
            yield break;
        }
		GameManager.Instance.DisableHandCards();
		yield return new WaitForSeconds(1);
        for (int col = 4; col >= 0; col--)
        {
            for (int row = 0; row < 2; row++)
			{
				if (Tile.plantTiles[row, col].planted != null && Tile.plantTiles[row, col].planted != this)
				{
					Destroy(Tile.plantTiles[row, col].planted.gameObject);
                    Pineclone card = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Pineclone")]).GetComponent<Pineclone>();
                    Tile.plantTiles[row, col].Plant(card);
                    card.suppress = true;
                }
			}
		}

        yield return null;
        yield return base.OnThisPlay();
	}

}
