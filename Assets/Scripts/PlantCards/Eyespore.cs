using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eyespore : Card
{

	protected override IEnumerator Fusion(Card parent)
	{
        if (Tile.GetTeamTiles(GetOpponent(team))[0, col].HasRevealedPlanted() || Tile.GetTeamTiles(GetOpponent(team))[1, col].HasRevealedPlanted())
        {
            yield return Glow();
            for (int i = 0; i < Tile.ROWS; i++) Tile.GetTeamTiles(GetOpponent(team))[i, col].planted.Destroy();
        }
        yield return base.Fusion(parent);
    }

}
