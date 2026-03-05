using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Starstruck : Buff
{

    private bool[] done = new bool[Tile.COLUMNS];

    protected override IEnumerator OnCardStatsChanged(Tuple<Card, int, int> changed)
    {
        if (changed.Item1.team != team && !done[changed.Item1.col])
        {
            for (int i = 0; i < Tile.ROWS; i++) if (Tile.GetTeamTiles(team)[i, changed.Item1.col].HasRevealedPlanted())
                {
                    done[changed.Item1.col] = true;
                    Tile.GetTeamTiles(team)[i, changed.Item1.col].planted.ChangeStats(Mathf.Max(0, changed.Item2), Mathf.Max(0, changed.Item3));
                }
            if (done[changed.Item1.col]) StartCoroutine(Glow());
        }
        return base.OnCardStatsChanged(changed);
    }

    protected override void AfterTurnEndBeforeTurnStart(object arg)
    {
        for (int i = 0; i < Tile.COLUMNS; i++) done[i] = false;
        base.AfterTurnEndBeforeTurnStart(arg);
    }  

}
