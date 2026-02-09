using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class MoodOfNovelty : Buff
{

    private int turns = 0;

	protected override IEnumerator OnTurnStart()
    {
        turns += 1;
        if (turns == 3)
        {
            Tile[,] target;
            if (team == Card.Team.A)
            {
                GameManager.Instance.plantPermanentAttackBonus += 3;
                GameManager.Instance.plantPermanentHPBonus += 3;
                target = Tile.plantTiles;
            }
            else
            {
                GameManager.Instance.zombiePermanentAttackBonus += 3;
                GameManager.Instance.zombiePermanentHPBonus += 3;
                target = Tile.zombieTiles;
            }
            for (int i = 0; i < 2; i++) for (int j = 0; j < 5; j++) if (target[i, j].HasRevealedPlanted()) target[i, j].planted.ChangeStats(3, 3);
            if (team == GameManager.Instance.team) foreach (HandCard hc in GameManager.Instance.GetHandCards()) if (hc.orig.type == Card.Type.Unit)
                    {
                        hc.ChangeAttack(0);
                        hc.ChangeHP(0);
                    }
        }
        yield return base.OnTurnStart();
    }

}
