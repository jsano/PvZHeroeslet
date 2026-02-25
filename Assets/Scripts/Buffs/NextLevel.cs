using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NextLevel : Buff
{

    private int turns = 0;

	protected override IEnumerator OnTurnStart()
    {
        turns += 1;
        if (turns == 3)
        {
            if (team == GameManager.Instance.team)
            {
                GameManager.Instance.playerPermanentAttackBonus += 3;
                GameManager.Instance.playerPermanentHPBonus += 3;
            }
            else
            {
                GameManager.Instance.opponentPermanentAttackBonus += 3;
                GameManager.Instance.opponentPermanentHPBonus += 3;
            }
            for (int i = 0; i < 2; i++) for (int j = 0; j < 5; j++) if (Tile.GetTeamTiles(team)[i, j].HasRevealedPlanted()) Tile.GetTeamTiles(team)[i, j].planted.ChangeStats(3, 3);
            if (team == GameManager.Instance.team) foreach (HandCard hc in GameManager.Instance.GetHandCards()) if (hc.orig.type == Card.Type.Unit)
                    {
                        hc.ChangeAttack(0);
                        hc.ChangeHP(0);
                    }
        }
        yield return base.OnTurnStart();
    }

}
