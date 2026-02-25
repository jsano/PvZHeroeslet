using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CobCannon : Card
{

    protected override IEnumerator OnThisPlay()
    {
        List<Card> targets = new();
        for (int i = -1; i <= 1; i++)
        {
            if (col + i < 0 || col + i > 4) continue;
            for (int r = 0; r < Tile.ROWS; r++) if (Tile.GetTeamTiles(GetOpponent(team))[r, col + i].HasRevealedPlanted()) targets.Add(Tile.GetTeamTiles(GetOpponent(team))[r, col + i].planted);
        }
        if (targets.Count > 0)
        {
            yield return Glow();
            foreach (Card c in targets) c.ChangeStats(-1, -1);
        }
        if (evolved)
        {
            for (int col = 0; col < 5; col++)
            {
                if (Tile.GetTeamTiles(GetOpponent(team))[0, col].HasRevealedPlanted() && !Tile.GetTeamTiles(GetOpponent(team))[0, col].planted.died)
                {
                    choices.Add(Tile.GetTeamTiles(GetOpponent(team))[0, col].GetComponent<BoxCollider2D>());
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
        bc.GetComponent<Tile>().planted.Destroy();
    }

}

