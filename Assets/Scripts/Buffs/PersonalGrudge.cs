using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PersonalGrudge : Buff
{

    protected override void Start()
    {
        for (int i = 0; i < Tile.ROWS; i++) for (int j = 0; j < Tile.COLUMNS; j++)
                if (Tile.GetTeamTiles(team)[i, j].HasRevealedPlanted() && Tile.GetTeamTiles(team)[i, j].planted.overshoot > 0)
                {
                    Tile.GetTeamTiles(team)[i, j].planted.overshoot += 1;
                }
        base.Start();
    }

    protected override IEnumerator OnCardPlay(Card played)
    {
        if (played.team == team && played.overshoot > 0) played.overshoot += 1;
        yield return base.OnCardPlay(played);
    }

}
