using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class FightOrFlight : Buff
{

    protected override IEnumerator OnCardPlay(Card played)
    {
        if (played.team == team)
        {
            if (played.type == Card.Type.Unit)
            {
                bool hasOpponentInCol = false;
                for (int r = 0; r < Tile.ROWS; r++)
                {
                    if (Tile.GetTeamTiles(Card.GetOpponent(played.team))[r, played.col].HasRevealedPlanted())
                    {
                        hasOpponentInCol = true;
                        break;
                    }
                }
                if (hasOpponentInCol) played.ChangeStats(2, 0);
            }
        }
        yield return base.OnCardPlay(played);
    }

}
