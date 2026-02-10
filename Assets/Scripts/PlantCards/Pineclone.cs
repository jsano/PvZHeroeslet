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
		yield return Glow();
        for (int col = 4; col >= 0; col--)
        {
            for (int row = 0; row < Tile.ROWS; row++)
			{
				if (Tile.GetTeamTiles(team)[row, col].planted != null && Tile.GetTeamTiles(team)[row, col].planted != this)
				{
                    Destroy(Tile.GetTeamTiles(team)[row, col].planted.gameObject);
                    Tile.GetTeamTiles(team)[row, col].Unplant();
                    Pineclone card = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Pineclone")]).GetComponent<Pineclone>();
                    Tile.GetTeamTiles(team)[row, col].Plant(card);
                    card.suppress = true;
                }
			}
		}

        yield return base.OnThisPlay();
	}

}
