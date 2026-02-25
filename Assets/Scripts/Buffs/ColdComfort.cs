using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColdComfort : Buff
{

    protected override IEnumerator OnTurnEnd()
    {
        for (int i = 0; i < Tile.ROWS; i++)
        {
            if (Tile.GetTeamTiles(team)[i, Tile.WATER].HasRevealedPlanted()) StartCoroutine(Tile.GetTeamTiles(team)[i, Tile.WATER].planted.Heal(1));
        }
        yield return base.OnTurnEnd();
    }

}
