using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SugarCrash : Buff
{

    protected override IEnumerator OnCardDeath(Tuple<Card, Card> died)
    {
        if (died.Item1.team == team)
        {
            int count = 0;
            for (int i = 0; i < Tile.ROWS; i++)
            {
                for (int j = 0; j < Tile.COLUMNS; j++)
                {
                    if (Tile.GetTeamTiles(team)[i, j].HasRevealedPlanted() && !Tile.GetTeamTiles(team)[i, j].planted.died) { 
                        Tile.GetTeamTiles(team)[i, j].planted.ChangeStats(0, 1);
                        count += 1;
                    }
                }
            }
            if (count > 0) StartCoroutine(Glow());
        }
        yield return base.OnCardDeath(died);
    }

}
