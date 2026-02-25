using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class IntergalacticWarlord : Card
{

    protected override IEnumerator OnThisPlay()
    {
        if (team == GameManager.Instance.team)
        {
            GameManager.Instance.playerPermanentAttackBonus += 1;
            GameManager.Instance.playerPermanentHPBonus += 1;
        }
        else
        {
            GameManager.Instance.opponentPermanentAttackBonus += 1;
            GameManager.Instance.opponentPermanentHPBonus += 1;
        }
        for (int r = 0; r < Tile.ROWS; r++) for (int j = 0; j < 5; j++) if (Tile.GetTeamTiles(team)[r, j].HasRevealedPlanted()) Tile.GetTeamTiles(team)[r, j].planted.ChangeStats(1, 1);
        foreach (HandCard hc in GameManager.Instance.GetHandCards()) if (hc.orig.type == Type.Unit)
            {
                hc.ChangeAttack(0);
                hc.ChangeHP(0);
            }
        yield return base.OnThisPlay();
    }

}
