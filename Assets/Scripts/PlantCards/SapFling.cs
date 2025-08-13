using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SapFling : Card
{

    protected override IEnumerator OnThisPlay()
    {
        for (int col = 1; col <= 3; col++)
        {
            choices.Add(Tile.terrainTiles[col].GetComponent<BoxCollider2D>());
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
        Tile t = bc.GetComponent<Tile>();
        yield return Glow();
        Card c = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Sappy Place")]);
        Tile.terrainTiles[t.col].Plant(c);
	}

}
