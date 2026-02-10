using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoodOfVolatility : Buff
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        if (team == GameManager.Instance.team)
        {
            GameManager.Instance.playerPermanentAttackBonus += 1;
        }
        else
        {
            GameManager.Instance.opponentPermanentAttackBonus += 1;
        }
        for (int i = 0; i < 2; i++) for (int j = 0; j < 5; j++) if (Tile.GetTeamTiles(team)[i, j].HasRevealedPlanted()) Tile.GetTeamTiles(team)[i, j].planted.ChangeStats(1, 0);
        if (team == GameManager.Instance.team) foreach (HandCard hc in GameManager.Instance.GetHandCards()) if (hc.orig.type == Card.Type.Unit)
            {
                hc.ChangeAttack(0);
                hc.ChangeHP(0);
            }
        base.Start();
    }

}
