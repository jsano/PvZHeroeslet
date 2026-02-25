using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinterMelon : Card
{

    protected override IEnumerator OnThisPlay()
    {
        for (int r = 0; r < Tile.ROWS; r++) for (int col = 0; col < Tile.COLUMNS; col++)
        {
            if (Tile.GetTeamTiles(GetOpponent(team))[r, col].HasRevealedPlanted())
            {
                choices.Add(Tile.GetTeamTiles(GetOpponent(team))[r, col].GetComponent<BoxCollider2D>());
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
        yield return Glow();
        bc.GetComponent<Tile>().planted.Freeze();
    }

}

