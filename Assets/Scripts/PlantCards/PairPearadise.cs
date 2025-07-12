using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PairPearadise : Card
{

	protected override IEnumerator OnCardPlay(Card played)
	{
        if (played.col == col && played.type == Type.Unit && played.team == Team.Plant)
        {
            if (Tile.CanPlantInCol(col, Tile.plantTiles, true, false))
            {
                yield return new WaitForSeconds(1);
                Card c = Instantiate(AllCards.InstanceToPrefab(played)).GetComponent<Card>();
                c.teamUp = true;
                Tile.plantTiles[1, col].Plant(c);
            }
        }
        
        yield return base.OnCardPlay(played);
	}

}
