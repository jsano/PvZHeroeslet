using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafBlower : Card
{

    protected override IEnumerator OnThisPlay()
    {
        if (Tile.terrainTiles[col].planted != null)
        {
            for (int i = 0; i < Tile.ROWS; i++) for (int col = 0; col < Tile.COLUMNS; col++)
            {
                if (Tile.GetTeamTiles(GetOpponent(team))[i, col].planted != null && Tile.GetTeamTiles(GetOpponent(team))[i, col].planted != this)
                {
                    choices.Add(Tile.GetTeamTiles(GetOpponent(team))[i, col].GetComponent<BoxCollider2D>());
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
        bc.GetComponent<Tile>().planted.Bounce();
    }

}

