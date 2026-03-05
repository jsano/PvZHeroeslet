using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClosingIn : Buff
{

    private bool destroyed = false;

    protected override IEnumerator OnCardDeath(Tuple<Card, Card> died)
    {
        if (died.Item1.team != team) destroyed = true;
        return base.OnCardDeath(died);
    }

    protected override IEnumerator OnTurnEnd()
    {
        if (!destroyed)
        {
            int highest = 0;
            for (int i = 0; i < Tile.ROWS; i++)
                for (int j = 0; j < Tile.COLUMNS; j++)
                    if (Tile.GetTeamTiles(Card.GetOpponent(team))[i, j].HasRevealedPlanted()) highest = Mathf.Max(Tile.GetTeamTiles(Card.GetOpponent(team))[i, j].planted.atk, highest);
            bool found = false;
            for (int i = 0; i < Tile.ROWS; i++)
            {
                if (found) break;
                for (int j = 0; j < Tile.COLUMNS; j++)
                    if (Tile.GetTeamTiles(Card.GetOpponent(team))[i, j].HasRevealedPlanted() && Tile.GetTeamTiles(Card.GetOpponent(team))[i, j].planted.atk == highest) 
                    {
                        found = true;
                        Tile.GetTeamTiles(Card.GetOpponent(team))[i, j].planted.Destroy();
                        StartCoroutine(Glow());
                        break;
                    }
            }
        }
        destroyed = false;
        yield return base.OnTurnEnd();
    }

}
