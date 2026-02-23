using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoodAghast : Buff
{

    protected override void OnBlock(Hero hero)
    {
        if (hero.team == team)
        {
            for (int i = 0; i < 2; i++) for (int j = 0; j < 5; j++) if (Tile.GetTeamTiles(team)[i, j].HasRevealedPlanted()) Tile.GetTeamTiles(team)[i, j].planted.ChangeStats(1, 1);
            if (team == GameManager.Instance.team)
            {
                GameManager.Instance.playerPermanentAttackBonus += 1;
                GameManager.Instance.playerPermanentHPBonus += 1;
                foreach (HandCard hc in GameManager.Instance.GetHandCards()) if (hc.orig.type == Card.Type.Unit)
                    {
                        hc.ChangeAttack(0);
                        hc.ChangeHP(0);
                    }
            }
            else
            {
                GameManager.Instance.opponentPermanentAttackBonus += 1;
                GameManager.Instance.opponentPermanentHPBonus += 1;
            }
        }
        base.OnBlock(hero);
    }

}
