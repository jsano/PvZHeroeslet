using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoodOfPassivity : Buff
{

    protected override IEnumerator OnTurnStart()
    {
        int count = 0;
        for (int i = 0; i < Tile.ROWS; i++)
        {
            for (int j = 0; j < Tile.COLUMNS; j++)
            {
                if (Tile.GetTeamTiles(team)[i, j].HasRevealedPlanted() && Tile.GetTeamTiles(team)[i, j].planted._class == Card.Class.Fright)
                {
                    Tile.GetTeamTiles(team)[i, j].planted.ToggleInvulnerability(true, true);
                    count++;
                }
            }
        }
        if (count > 0) StartCoroutine(Glow());
        yield return base.OnTurnStart();
    }

    protected override void OnCardPlayImmediate(Card played)
    {
        if (played.team == team && played._class == Card.Class.Fright)
        {
            StartCoroutine(Glow());
            played.ToggleInvulnerability(true, true);
        }
        base.OnCardPlayImmediate(played);
    }

}
