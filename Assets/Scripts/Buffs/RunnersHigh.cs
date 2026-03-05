using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RunnersHigh : Buff
{

    protected override IEnumerator OnTurnEnd()
    {
        int mine = 0;
        int opponent = 0;
        for (int i = 0; i < Tile.ROWS; i++) for (int j = 0; j < Tile.COLUMNS; j++)
        {
            if (Tile.GetTeamTiles(team)[i, j].HasRevealedPlanted()) mine += 1;
            if (Tile.GetTeamTiles(Card.GetOpponent(team))[i, j].HasRevealedPlanted()) opponent += 1;
        }
        if (mine > opponent)
        {
            StartCoroutine(Glow());
            yield return GameManager.Instance.DrawCard(team);
        }
        yield return base.OnTurnEnd();
    }

}
