using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoodOfAttrition : Buff
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        Tile[,] target;
        if (team == Card.Team.Plant)
        {
            GameManager.Instance.plantPermanentHPBonus += 2;
            target = Tile.plantTiles;
        }
        else
        {
            GameManager.Instance.zombiePermanentHPBonus += 2;
            target = Tile.zombieTiles;
        }
        for (int i = 0; i < 2; i++) for (int j = 0; j < 5; j++) if (target[i, j].HasRevealedPlanted()) target[i, j].planted.ChangeStats(0, 2);
        if (team == GameManager.Instance.team) foreach (HandCard hc in GameManager.Instance.GetHandCards()) if (hc.orig.type == Card.Type.Unit)
            {
                hc.ChangeAttack(0);
                hc.ChangeHP(0);
            }
        base.Start();
    }

}
