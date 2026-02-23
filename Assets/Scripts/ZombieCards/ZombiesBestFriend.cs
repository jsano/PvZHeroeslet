using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ZombiesBestFriend : Card
{

	protected override IEnumerator OnThisPlay()
	{
        if (col > 0 && (Tile.GetTeamTiles(team)[0, col - 1].HasRevealedPlanted() || Tile.GetTeamTiles(team)[1, col - 1].HasRevealedPlanted()) ||
            col < 4 && (Tile.GetTeamTiles(team)[0, col + 1].HasRevealedPlanted() || Tile.GetTeamTiles(team)[1, col + 1].HasRevealedPlanted()))
        {
            for (int row = 0; row < 2; row++) for (int col = 0; col < 4; col++)
            {
                if (Tile.CanPlantInCol(col, Tile.GetTeamTiles(team), false, false))
                {
                    choices.Add(Tile.GetTeamTiles(team)[row, col].GetComponent<BoxCollider2D>());
                }
            }
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
        yield return Glow();
        Tile t = bc.GetComponent<Tile>();
        yield return SyncRandomChoiceAcrossNetwork(AllCards.RandomFromCost((1, 1), true) + "");
        Card c = Instantiate(AllCards.Instance.cards[int.Parse(GameManager.Instance.GetShuffledList()[0])]);
        Tile.GetTeamTiles(team)[t.row, t.col].Plant(c);
    }

}
