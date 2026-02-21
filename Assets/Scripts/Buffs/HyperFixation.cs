using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HyperFixation : Buff
{

    private HashSet<Card> affected = new();

    protected override IEnumerator OnTurnEnd()
    {
        List<Card> mine = new();
        for (int i = 0; i < Tile.ROWS; i++) for (int j = 0; j < Tile.COLUMNS; j++)
                if (Tile.GetTeamTiles(team)[i, j].HasRevealedPlanted()) mine.Add(Tile.GetTeamTiles(team)[i, j].planted);
        if (mine.Count == 1 && !affected.Contains(mine[0]))
        {
            mine[0].doubleStrike += 1;
            affected.Add(mine[0]);
        }
        yield return base.OnTurnEnd();
    }

}
