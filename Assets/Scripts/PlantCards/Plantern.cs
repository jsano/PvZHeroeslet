using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Plantern : Card
{

    private bool buffed;

	protected override IEnumerator OnThisPlay()
	{
		if (Tile.terrainTiles[col].planted != null)
		{
			ChangeStats(1, 1);
            bullseye += 1;
            buffed = true;
		}
		yield return base.OnThisPlay();
	}

    protected override IEnumerator OnCardPlay(Card played)
    {
        if (played.type == Type.Terrain && !buffed)
        {
            ChangeStats(1, 1);
            bullseye += 1;
            buffed = true;
        }
        yield return base.OnCardPlay(played);
    }

    protected override IEnumerator OnCardMoved(Card moved)
	{
		if (moved == this)
		{
            if (Tile.terrainTiles[col].planted == null && buffed)
            {
                ChangeStats(-1, -1);
                bullseye -= 1;
                buffed = false;
            }
            else if (Tile.terrainTiles[col].planted != null && !buffed)
            {
                ChangeStats(1, 1);
                bullseye += 1;
                buffed = true;
            }
        }
		yield return base.OnCardMoved(moved);
	}

}