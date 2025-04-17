using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bluesberry : Card
{

	protected override IEnumerator OnThisPlay()
	{
        for (int col = 0; col < 5; col++)
        {
            if (Tile.zombieTiles[0, col].HasRevealedPlanted()) choices.Add(Tile.zombieTiles[0, col].planted.GetComponent<BoxCollider2D>());
        }
		if (GameManager.Instance.team == team)
		{
            choices.Add(GameManager.Instance.zombieHero.GetComponent<BoxCollider2D>());
			if (choices.Count == 1) StartCoroutine(OnSelection(choices[0]));
			if (choices.Count >= 2)
            {
                selected = false;
            }
		}
        if (choices.Count > 0) GameManager.Instance.selecting = true;
		yield return base.OnThisPlay();
	}

	protected override IEnumerator OnSelection(BoxCollider2D bc)
	{
        yield return new WaitForSeconds(1);
		Card c = bc.GetComponent<Card>();
        if (c == null) yield return GameManager.Instance.zombieHero.ReceiveDamage(2, this);
        else yield return c.ReceiveDamage(2, this);
        GameManager.Instance.EndSelectingRpc();
    }

}
