using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafBlower : Card
{

    protected override IEnumerator OnThisPlay()
    {
        if (Tile.terrainTiles[col].planted != null)
        {
            for (int col = 0; col < 5; col++)
            {
                if (Tile.zombieTiles[0, col].planted != null && Tile.zombieTiles[0, col].planted != this)
                {
                    choices.Add(Tile.zombieTiles[0, col].GetComponent<BoxCollider2D>());
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
        yield return new WaitForSeconds(1);
        bc.GetComponent<Tile>().planted.Bounce();
    }

}

