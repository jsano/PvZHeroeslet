using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LastStand : Buff
{

    private bool activated = false;

    protected override IEnumerator OnCardHurt(Tuple<Damagable, Card, int, int> hurt)
    {
        if (hurt.Item1 == GameManager.Instance.GetTeamHero(team) && ((Hero)(hurt.Item1)).HP <= 10 && !activated)
        {
            activated = true;
            for (int i = 0; i < 2; i++) for (int j = 0; j < 5; j++) if (Tile.GetTeamTiles(team)[i, j].HasRevealedPlanted()) Tile.GetTeamTiles(team)[i, j].planted.ChangeStats(3, 0);
            if (team == GameManager.Instance.team)
            {
                GameManager.Instance.playerPermanentAttackBonus += 3;
                foreach (HandCard hc in GameManager.Instance.GetHandCards()) if (hc.orig.type == Card.Type.Unit)
                    {
                        hc.ChangeAttack(0);
                        hc.ChangeHP(0);
                    }
            }
            else GameManager.Instance.opponentPermanentAttackBonus += 3;
            StartCoroutine(Glow());
        }
        yield return base.OnCardHurt(hurt);
    }

}
