using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinterMelon : Card
{

    protected override IEnumerator OnThisPlay()
    {
        if (GameManager.Instance.team == team)
        {
            GameManager.Instance.DisableHandCards();
            for (int col = 0; col < 5; col++)
            {
                if (Tile.zombieTiles[0, col].planted != null)
                {
                    choices.Add(Tile.zombieTiles[0, col].planted.GetComponent<BoxCollider2D>());
                }
            }
            if (choices.Count == 1) yield return OnSelection(choices[0]);
            if (choices.Count >= 2)
            {
                selected = false;
            }
        }
        GameManager.Instance.selecting = true;
        yield return base.OnThisPlay();
    }

    protected override IEnumerator OnSelection(BoxCollider2D bc)
    {
        yield return new WaitForSeconds(1);
        Card c = bc.GetComponent<Card>();
        GameManager.Instance.FreezeRpc(c.team, c.row, c.col);
    }

}

