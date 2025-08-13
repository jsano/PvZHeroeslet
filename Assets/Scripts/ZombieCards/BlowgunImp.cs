using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowgunImp : Card
{

    protected override IEnumerator OnThisPlay()
    {
        if (evolved)
        {
            for (int i = 0; i < 2; i++) for (int col = 0; col < 5; col++)
            {
                if (Tile.plantTiles[i, col].HasRevealedPlanted() && Tile.plantTiles[i, col].planted != this)
                {
                    choices.Add(Tile.plantTiles[i, col].GetComponent<BoxCollider2D>());
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

