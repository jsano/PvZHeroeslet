using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IronBoarder : Card
{

    private bool buffed;

	protected override IEnumerator OnThisPlay()
	{
		if (Tile.terrainTiles[col].planted != null)
		{
			ChangeStats(1, 1);
            buffed = true;
		}
		yield return base.OnThisPlay();
	}

    protected override IEnumerator OnCardPlay(Card played)
    {
        if (played.type == Type.Terrain && played.col == col && !buffed)
        {
            ChangeStats(1, 1);
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
                buffed = false;
            }
            else if (Tile.terrainTiles[col].planted != null && !buffed)
            {
                ChangeStats(1, 1);
                buffed = true;
            }
        }
		yield return base.OnCardMoved(moved);
	}

    protected override IEnumerator OnCardBounce(Card bounced)
    {
        if (Tile.terrainTiles[col].planted == null && buffed)
        {
            ChangeStats(-1, -1);
            buffed = false;
        }
        yield return base.OnCardBounce(bounced);
    }

}