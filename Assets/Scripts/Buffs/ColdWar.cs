using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColdWar : Buff
{

    protected override IEnumerator OnCardPlay(Card played)
    {
        if (played.team != team && played.type == Card.Type.Unit &&
            Tile.GetTeamTiles(Card.GetOpponent(team))[0, played.col].HasRevealedPlanted() && Tile.GetTeamTiles(Card.GetOpponent(team))[1, played.col].HasRevealedPlanted())
        {
            StartCoroutine(Glow());
            Tile.GetTeamTiles(Card.GetOpponent(team))[0, played.col].planted.ChangeStats(-2, 0);
        }
        yield return base.OnCardPlay(played);
    }

}
