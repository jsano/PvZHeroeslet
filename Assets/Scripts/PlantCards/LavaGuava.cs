using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaGuava : Card
{

	protected override IEnumerator OnThisPlay()
	{
		
		yield return new WaitForSeconds(1);
        for (int i = -1; i <= 1; i++)
        {
            if (col + i < 0 || col + i > 4) continue;
            if (Tile.zombieTiles[0, col + i].HasRevealedPlanted() && Tile.zombieTiles[0, col + i].planted.untrickable == 0) StartCoroutine(Tile.zombieTiles[0, col + i].planted.ReceiveDamage(2, this));
        }
        if (col >= 1 && col <= 3)
        {
            Card c = Instantiate(AllCards.Instance.cards[AllCards.NameToID("Hot Lava")]);
            Tile.terrainTiles[col].Plant(c);
        }
		yield return base.OnThisPlay();
	}

    public override bool IsValidTarget(BoxCollider2D bc)
    {
        if (!base.IsValidTarget(bc)) return false;
        Tile t = bc.GetComponent<Tile>();
        if (t != null && t.isTerrainTile) return true;
        return false;
    }

}
