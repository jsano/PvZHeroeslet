using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoodOfDisdain : Buff
{

    protected override IEnumerator OnCardPlay(Card played)
    {
        if (played.team != team && played.type == Card.Type.Unit) 
        {
            int count = 0;
            for (int i = 0; i < Tile.ROWS; i++) for (int j = 0; j < Tile.COLUMNS; j++) 
            {
                Tile t = Tile.GetTeamTiles(team)[i, j];
                if (t.HasRevealedPlanted() && t.planted._class == Card.Class.Contempt) count++;
            }
            if (count >= 3)
            {
                StartCoroutine(Glow());
                played.ChangeStats(-1, -1);
            }
        }
        yield return base.OnCardPlay(played);
    }

}
